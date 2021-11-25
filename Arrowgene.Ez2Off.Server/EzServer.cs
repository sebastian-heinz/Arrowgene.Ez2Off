/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2020 Sebastian Heinz
 *
 * Github: https://github.com/Arrowgene/Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ez2Off is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ez2Off. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Chat;
using Arrowgene.Ez2Off.Server.Chat.Command;
using Arrowgene.Ez2Off.Server.Chat.Command.Commands;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Database.Sql;
using Arrowgene.Ez2Off.Server.Logs;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Plugin;
using Arrowgene.Ez2Off.Server.Sessions;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Ez2Off.Server.Tasks;
using Arrowgene.Ez2Off.Server.Trait;
using Arrowgene.Logging;
using Arrowgene.Networking;
using Arrowgene.Networking.Tcp.Server;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;
using Arrowgene.Services.Tasks;

namespace Arrowgene.Ez2Off.Server
{
    public class EzServer
    {
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ILogger logger = LogProvider.Logger(typeof(EzServer));

            logger.Error("Unhandled Exception}");
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                logger.Exception(ex);
            }
        }
        
        public const int NoExpectedSize = -1;

        public static readonly IBufferProvider Buffer = new StreamBuffer();

        private readonly EzLogger _logger;
        private readonly ITcpServer _loginServer;
        private readonly ITcpServer _gameServer;
        private readonly Channel[] _rubyChannels;
        private readonly Channel[] _streetChannels;
        private readonly Channel[] _clubChannels;
        private readonly Dictionary<int, Song> _songs;
        private readonly Dictionary<int, Radiomix> _radiomixes;
        private readonly IProvider _provider;
        private readonly List<ServerPoint> _serverPoints;
        private ServerTrait _serverTrait;
        private volatile bool _isRunning;

        public PluginDispatcher PluginDispatcher { get; }
        public ServerPoint ServerPoint { get; }
        public EzQueueConsumer LoginConsumer { get; }
        public EzQueueConsumer GameConsumer { get; }
        public EzSettings Settings { get; }
        public ClientLookup Clients { get; }
        public PacketRouter Router { get; }
        public SessionManager Sessions { get; }
        public TaskManager Tasks { get; }
        public IDatabase Database { get; }
        public long StartupTime { get; }
        public ChatManager Chat { get; private set; }
        public bool IsRunning => _isRunning;

        public EzServer(EzSettings settings, IProvider provider)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            
            Settings = new EzSettings(settings);
            _provider = provider;

            LogProvider.Configure<EzLogger>(Settings);
            _logger = LogProvider.Instance.GetLogger<EzLogger>(this);

            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;

            _isRunning = false;

            PluginDispatcher = new PluginDispatcher();

            IPacketFactoryProvider packetFactoryProvider = provider.ProvidePacketFactoryProvider(Settings);
            LoginConsumer =
                new EzQueueConsumer(packetFactoryProvider, Settings.LoginSocketServerSettings.MaxUnitOfOrder);
            LoginConsumer.SetIdentity(Settings.LoginSocketServerSettings.Identity);
            LoginConsumer.ClientDisconnected = LoginServer_OnClientDisconnected;
            LoginConsumer.Started = LoginServer_Started;

            GameConsumer = new EzQueueConsumer(packetFactoryProvider, Settings.GameSocketServerSettings.MaxUnitOfOrder);
            GameConsumer.SetIdentity(Settings.GameSocketServerSettings.Identity);
            GameConsumer.ClientDisconnected = GameServer_OnClientDisconnected;
            GameConsumer.ClientConnected = GameServer_OnClientConnected;
            GameConsumer.Started = GameServer_Started;

            _loginServer = new AsyncEventServer(
                Settings.ListenIpAddress,
                Settings.LoginPort,
                LoginConsumer,
                Settings.LoginSocketServerSettings
            );
            _gameServer = new AsyncEventServer(
                Settings.ListenIpAddress,
                Settings.GamePort,
                GameConsumer,
                Settings.GameSocketServerSettings
            );

            _rubyChannels = new Channel[Channel.MaxChannels];
            _streetChannels = new Channel[Channel.MaxChannels];
            _clubChannels = new Channel[Channel.MaxChannels];
            _songs = new Dictionary<int, Song>();
            _radiomixes = new Dictionary<int, Radiomix>();
            ServerPoint = new ServerPoint
            {
                Id = 0,
                Public = new NetworkPoint(Settings.GameIpAddress, Settings.GamePort)
            };
            _serverPoints = new List<ServerPoint>();

            StartupTime = Utils.GetUnixTime(DateTime.Now);
            Clients = new ClientLookup();
            Router = new PacketRouter();
            Sessions = new SessionManager();
            Tasks = new TaskManager();

            switch (Settings.DatabaseSettings.Type)
            {
                case DatabaseType.SQLite:
                    Database = new SqLiteDb(Settings.DatabaseSettings.SqLitePath);
                    break;
                case DatabaseType.Maria:
                    Database = new MariaDb(
                        Settings.DatabaseSettings.MariaHost,
                        Settings.DatabaseSettings.MariaPort,
                        Settings.DatabaseSettings.MariaUser,
                        Settings.DatabaseSettings.MariaPassword,
                        Settings.DatabaseSettings.MariaDatabase
                    );
                    break;
                default:
                    _logger.Error("No database specified!");
                    Environment.Exit(1);
                    break;
            }
        }

        public void Start()
        {
            _isRunning = true;

            _serverTrait = _provider.ProvideServerTrait(this);
            Chat = new ChatManager(_provider.ProvideChatTrait(this));

            ChannelTrait channelTrait = _provider.ProvideChannelTrait(this);
            RoomTrait roomTrait = _provider.ProvideRoomTrait(this);
            for (int i = 0; i < Channel.MaxChannels; i++)
            {
                _rubyChannels[i] = new Channel(i, channelTrait, roomTrait, this);
                _streetChannels[i] = new Channel(i, channelTrait, roomTrait, this);
                _clubChannels[i] = new Channel(i, channelTrait, roomTrait, this);
            }

            foreach (IHandler loginHandler in _provider.ProvideLoginHandler(this))
            {
                LoginConsumer.AddHandler(loginHandler);
            }

            foreach (IHandler worldHandler in _provider.ProvideWorldHandler(this))
            {
                GameConsumer.AddHandler(worldHandler);
            }

            List<Song> songs = Database.SelectSongs();
            foreach (Song song in songs)
            {
                _songs.Add(song.Id, song);
            }

            List<Radiomix> radiomixes = Database.SelectRadiomixes();
            foreach (Radiomix radiomix in radiomixes)
            {
                _radiomixes.Add(radiomix.Id, radiomix);
            }

            Tasks.AddTask(new UpdateStatusTask(Database));
            Tasks.AddTask(new CleanSessionsTask(Sessions));
            Tasks.AddTask(new RemoveExpiredItems(this));
            Tasks.AddTask(new RemoveExpiredGifts(this));
            Tasks.AddTask(new LogStatus(this));

            ChatCommand chatCommand = new ChatCommand(this);
            foreach (BaseChatCommand command in _provider.ProvideChatCommands(this))
            {
                chatCommand.AddCommand(command);
            }

            Chat.AddMiddleware(chatCommand);
            PluginDispatcher.Start();

            _serverPoints.Add(ServerPoint);

            _logger.Info($"Startup: {StartupTime}");
            _logger.Info($"OS: {Environment.OSVersion}");
            _logger.Info($"x64 OS: {Environment.Is64BitOperatingSystem}");
            _logger.Info($"Processors: {Environment.ProcessorCount}");
            _logger.Info($".NET Version: {Environment.Version}");
            _logger.Info($"x64 Process: {Environment.Is64BitProcess}");
            _logger.Info($"CurrentDirectory: {Environment.CurrentDirectory}");
            _logger.Info($"ApplicationDirectory: {Utils.ExecutingDirectory()}");
            _logger.Info($"RelativeApplicationDirectory: {Utils.RelativeExecutingDirectory()}");
            _logger.Info($"Login Handler: {LoginConsumer.HandlersCount}");
            _logger.Info($"Login Server: {_loginServer.IpAddress}:{_loginServer.Port}");
            _logger.Info($"World Handler: {GameConsumer.HandlersCount}");
            _logger.Info($"World Server: {_gameServer.IpAddress}:{_gameServer.Port}");
            _logger.Info($"Setting CombineChannel: {Settings.CombineChannel}");
            _logger.Info($"Setting LogLevel: {Settings.LogLevel}");
            _logger.Info($"Setting NeedCharacter: {Settings.NeedCharacter}");
            _logger.Info($"Setting NeedRegistration: {Settings.NeedRegistration}");

            _gameServer.Start();
        }

        private void GameServer_Started()
        {
            if (!_isRunning)
            {
                return;
            }

            _loginServer.Start();
            Tasks.Start();
        }

        private void LoginServer_Started()
        {
            if (!_isRunning)
            {
                return;
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _logger.Info("Shutdown: Starting to shutdown server...");
            _loginServer.Stop();
            Chat.SendGmNotice("Server will be shutdown...", Clients.GetAllClients());
            Thread.Sleep(1000);
            _logger.Info("Shutdown: Login server closed");

            List<Room> rooms = new List<Room>();
            for (int i = 0; i < Channel.MaxChannels; i++)
            {
                rooms.AddRange(_rubyChannels[i].GetRooms());
                rooms.AddRange(_streetChannels[i].GetRooms());
                rooms.AddRange(_clubChannels[i].GetRooms());
            }

            bool closingRooms = true;
            List<Room> activeRooms = new List<Room>();
            while (closingRooms)
            {
                _logger.Info($"Shutdown: Waiting for {rooms.Count} rooms to finish playing...");
                foreach (Room room in rooms)
                {
                    if (room.Playing)
                    {
                        activeRooms.Add(room);
                    }
                    else
                    {
                        room.Close();
                    }
                }

                if (activeRooms.Count > 0)
                {
                    rooms = activeRooms;
                    activeRooms.Clear();
                    Thread.Sleep(1000 * 30);
                }
                else
                {
                    closingRooms = false;
                }
            }

            _logger.Info("Shutdown: All rooms finished");

            if (Clients.GetAllClients().Count > 0)
            {
                for (int i = 10; i > 1; i--)
                {
                    Chat.SendGmNotice($"Server will be shutdown in {i} seconds...", Clients.GetAllClients());
                    Thread.Sleep(1000);
                }
            }

            Chat.SendGmNotice($"Shutdown NOW!", Clients.GetAllClients());
            Thread.Sleep(2000);

            _logger.Info("Shutdown: Disconnecting all clients...");
            List<EzClient> clients = Clients.GetAllClients();
            foreach (EzClient client in clients)
            {
                client.Socket.Close();
            }

            _logger.Info("Shutdown: Stopping game server...");
            Tasks.Stop();
            _gameServer.Stop();

            _songs.Clear();
            _radiomixes.Clear();
            GameConsumer.Clear();
            LoginConsumer.Clear();
            Tasks.Clear();
            PluginDispatcher.Stop();

            _logger.Info("Shutdown: Completed");
        }

        public List<ServerPoint> GetServerPoints()
        {
            return new List<ServerPoint>(_serverPoints);
        }

        public ServerPoint GetServerPoint(int serverPointId)
        {
            foreach (ServerPoint serverPoint in _serverPoints)
            {
                if (serverPoint.Id == serverPointId)
                {
                    return serverPoint;
                }
            }

            return null;
        }

        public Channel GetChannel(ModeType mode, int index)
        {
            if (index > Channel.MaxChannels)
            {
                return null;
            }

            switch (mode)
            {
                case ModeType.RubyMix: return _rubyChannels[index];
                case ModeType.StreetMix: return _streetChannels[index];
                case ModeType.ClubMix: return _clubChannels[index];
            }

            return null;
        }

        public Channel[] GetChannels(ModeType mode)
        {
            switch (mode)
            {
                case ModeType.RubyMix: return _rubyChannels;
                case ModeType.StreetMix: return _streetChannels;
                case ModeType.ClubMix: return _clubChannels;
            }

            return null;
        }

        /// <summary>
        /// Returns a Song by its id or null if it doesn't exist.
        /// </summary>
        public Song GetSong(int songId)
        {
            if (_songs.ContainsKey(songId))
            {
                return _songs[songId];
            }

            return null;
        }

        /// <summary>
        /// Returns a Song by its id or null if it doesn't exist.
        /// </summary>
        public Radiomix GetRadiomix(int radiomixId)
        {
            if (_radiomixes.ContainsKey(radiomixId))
            {
                return _radiomixes[radiomixId];
            }

            return null;
        }

        /// <summary>
        /// Returns a Character by its name or null if it does not exist.
        /// 
        /// If the Character is online the instance will be returned,
        /// otherwise a new instance will be created from the database.
        /// </summary>
        public Character GetCharacter(string characterName)
        {
            Character character;
            EzClient client = Clients.GetClient(characterName);
            if (client != null)
            {
                character = client.Character;
            }
            else
            {
                character = Database.SelectCharacter(characterName);
            }

            return character;
        }

        private void LoginServer_OnClientDisconnected(EzClient client)
        {
            Task.Delay(TimeSpan.FromSeconds(3)).ContinueWith(t =>
            {
                Session session = client.Session;
                if (session != null)
                {
                    _logger.Info(client, $"Deleting session: {session.Key}");
                    Sessions.DeleteSession(session.Key);
                }
            });
        }

        private void GameServer_OnClientConnected(EzClient client)
        {
            _logger.Info(client, "GameServer: Client Connected");
        }

        public void GameServer_OnClientAuthenticated(EzClient client, Session session)
        {
            client.Session = session;
            client.UpdateIdentity();
            _logger.Info(client, "GameServer: Client Authenticated");
            Clients.AddClient(client);
            ServerPoint.ChangeLoad(client.Mode, 1);
        }

        private void GameServer_OnClientDisconnected(EzClient client)
        {
            _logger.Info(client, "GameServer: Client Disconnected");
            Clients.RemoveClient(client);

            if (client.Session == null)
            {
                _logger.Debug(client, "Client disconnected without valid session");
                return;
            }

            ServerPoint.ChangeLoad(client.Mode, -1);

            Database.UpsertCharacter(client.Character);
            Database.UpsertSetting(client.Setting);

            Player player = client.Player;
            if (player != null)
            {
                player.Playing = false;
            }


            Room room = client.Room;
            if (room != null)
            {
                try
                {
                    room.FinishGame(client);
                }
                catch (Exception ex)
                {
                    _logger.Exception(client, ex);
                }

                try
                {
                    room.Leave(client);
                }
                catch (Exception ex)
                {
                    _logger.Exception(client, ex);
                }
            }

            if (client.Channel != null)
            {
                try
                {
                    Channel channel = client.Channel;
                    channel.Leave(client);
                }
                catch (Exception ex)
                {
                    _logger.Exception(client, ex);
                }
            }

            _serverTrait.ClientDisconnected(client);
        }

        private void CurrentDomainOnProcessExit(object sender, EventArgs e)
        {
            Stop();
        }
    }
}