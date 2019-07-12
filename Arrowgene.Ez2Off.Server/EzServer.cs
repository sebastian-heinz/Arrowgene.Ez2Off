/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2018 Sebastian Heinz
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
using System.IO;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Database.SQLite;
using Arrowgene.Ez2Off.Server.Log;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Scripting;
using Arrowgene.Ez2Off.Server.Sessions;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.ServerBridge;
using Arrowgene.Services.Networking.Tcp;
using Arrowgene.Services.Networking.Tcp.Consumer;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ez2Off.Server
{
    public abstract class EzServer : IConsumer
    {
        public const int MaxChannels = 10;

        public static readonly IBufferProvider Buffer = new StreamBuffer();

        private readonly AsyncEventServer _server;
        private readonly Dictionary<int, IHandler> _handlers;

        protected readonly EzLogger _logger;

        public static string GetVersion()
        {
            Version version = Utils.GetAssemblyVersion("Arrowgene.Ez2Off");
            if (version != null && version.Major > 0)
            {
                return version.ToString();
            }

            return Utils.DefaultVersion;
        }

        public EzServer(EzServerSettings settings)
        {
            Settings = settings;
            LogProvider<EzLogger>.Provider.Configure(Settings);
            _logger = LogProvider<EzLogger>.GetLogger(this);
            _handlers = new Dictionary<int, IHandler>();
            _server = new AsyncEventServer(Settings.ListenIpAddress, Settings.Port, this, Settings.ServerSettings);
            Clients = new ClientLookup();
            Database = new SQLiteDb(settings.DatabaseSettings.SQLitePath);
            SessionManager = new SessionManager();
        }

        public abstract string Name { get; }
        public abstract IBridge Bridge { get; }

        public ISessionManager SessionManager { get; }
        public EzServerSettings Settings { get; }
        public IDatabase Database { get; }
        public ClientLookup Clients { get; }

        protected abstract void LoadHandles();

        public void Start()
        {
            if (Settings.Active)
            {
                _handlers.Clear();
                LoadHandles();
                LoadScriptHandles();
                _logger.Info("Loaded {0} handles", _handlers.Count);
                _logger.Info("Listening: {0} Public: {1} Port: {2}", _server.IpAddress, Settings.PublicIpAddress,
                    _server.Port);
                _Start();
                Bridge.Start();
                _server.Start();
            }
        }

        public void Stop()
        {
            if (Settings.Active)
            {
                Bridge.Stop();
                _server.Stop();
                _Stop();
            }
        }

        public void ReloadScripts()
        {
            LoadScriptHandles();
        }
        
        public void Send(EzClient client, EzPacket packet)
        {
            _logger.LogOutgoingPacket(client, packet);
            client.Send(packet);
        }

        public void Send(List<EzClient> clients, EzPacket packet)
        {
            foreach (EzClient client in clients)
            {
                Send(client, packet);
            }
        }

        public void Send(EzClient client, byte id, IBuffer data)
        {
            EzPacket packet = new EzPacket(id, data);
            Send(client, packet);
        }

        public void Send(List<EzClient> clients, byte id, IBuffer data)
        {
            foreach (EzClient client in clients)
            {
                Send(client, id, data);
            }
        }

        public void Send(Channel channel, byte id, IBuffer data)
        {
            Send(channel.GetClients(), id, data);
        }

        public void Send(Room room, byte id, IBuffer data)
        {
            Send(room.GetClients(), id, data);
        }

        protected virtual void _Start()
        {
        }

        protected virtual void _Stop()
        {
        }

        protected virtual void OnClientDisconnected(EzClient client)
        {
        }

        protected virtual void OnClientConnected(EzClient client)
        {
        }

        protected void AddHandler(IHandler handler)
        {
            if (_handlers.ContainsKey(handler.Id))
            {
                _handlers[handler.Id] = handler;
                _logger.Info("Hander Id: {0} was overwritten", handler.Id);
            }
            else
            {
                _handlers.Add(handler.Id, handler);
            }
        }

        private void LoadScriptHandles()
        {
            if (!Settings.LoadHandlerScripts)
            {
                _logger.Info("ScriptHandler loading is disabled");
                return;
            }

            if (!Directory.Exists(Settings.HandlerScriptsPath))
            {
                _logger.Error(
                    "'HandlerScriptsPath' property ({0}) inside server_config.json is not a valid directory.",
                    Settings.HandlerScriptsPath);
                return;
            }

            DirectoryInfo scriptHandlerDirectory = new DirectoryInfo(Settings.HandlerScriptsPath);
            List<IHandler> handlers = EzScriptEngine.Instance.LoadHandlers(scriptHandlerDirectory, this);
            foreach (IHandler handler in handlers)
            {
                AddHandler(handler);
            }

            _logger.Info("Loaded {0} script handles from {1}", handlers.Count, scriptHandlerDirectory.FullName);
        }

        void IConsumer.OnReceivedData(ITcpSocket socket, byte[] data)
        {
            EzClient client = Clients.GetClient(socket);
            EzPacket packet = client.Read(data);
            if (packet != null)
            {
                if (_handlers.ContainsKey(packet.Id))
                {
                    _logger.LogIncomingPacket(client, packet);
                    packet.Data.SetPositionStart();
                    try
                    {
                        _handlers[packet.Id].Handle(client, packet);
                    }
                    catch (Exception ex)
                    {
                        _logger.Exception(ex);
                    }
                }
                else
                {
                    _logger.LogUnknownIncommingPacket(client, packet);
                }
            }
        }

        void IConsumer.OnClientDisconnected(ITcpSocket socket)
        {
            EzClient client = Clients.GetClient(socket);
            Clients.RemoveClient(client);
            _logger.Info("Client: {0} disconnected", client.Identity);
            OnClientDisconnected(client);
        }

        void IConsumer.OnClientConnected(ITcpSocket socket)
        {
            EzClient client = new EzClient(socket);
            Clients.AddClient(client);
            _logger.Info("Client: {0} connected", client.Identity);
            OnClientConnected(client);
        }

        void IConsumer.OnStart()
        {
        }

        void IConsumer.OnStop()
        {
        }
    }
}