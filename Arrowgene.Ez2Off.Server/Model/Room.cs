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
using System.Threading.Tasks;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Logs;
using Arrowgene.Ez2Off.Server.Trait;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.Server.Model
{
    /// <summary>
    /// A thread safe room.
    /// </summary>
    public class Room
    {
        public const int MaxSlots = 8;
        public const int NewRoomMasterSlot = 0;

        private readonly EzClient[] _clients;
        private readonly object _lock;
        private readonly RoomTrait _trait;
        private readonly EzLogger _logger;
        private readonly EzServer _server;

        private bool _isOpen;
        private bool _isStarting;

        public Channel Channel { get; }
        public bool AllowViewer { get; set; }
        public bool PasswordProtected { get; set; }
        public byte Number { get; set; }
        public byte MaxPlayer { get; set; }
        public int SelectedSong { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DifficultyType MaxDifficulty { get; set; }
        public DifficultyType Difficulty { get; set; }
        public GameGroupType GameGroupType { get; set; }
        public GameType GameType { get; set; }
        public bool RandomSong { get; set; }
        public NoteEffectType NoteEffect { get; set; }
        public FadeEffectType FadeEffect { get; set; }
        public ModeType Mode { get; set; }
        public Game Game { get; private set; }
        public bool Playing { get; private set; }
        public EzClient Master { get; private set; }

        public Room(byte id, RoomInfo info, EzClient master, RoomTrait trait, EzServer server)
        {
            _logger = LogProvider.Logger<EzLogger>(this);
            _trait = trait;
            _lock = new object();
            _clients = new EzClient[MaxSlots];
            _isOpen = true;
            _isStarting = false;
            _server = server;

            AllowViewer = info.AllowViewer;
            PasswordProtected = info.PasswordProtected;
            Number = id;
            MaxPlayer = info.MaxPlayer;
            SelectedSong = info.SelectedSong;
            Name = info.Name;
            Password = info.Password;
            MaxDifficulty = info.MaxDifficulty;
            Difficulty = info.Difficulty;
            GameGroupType = info.GameGroupType;
            GameType = info.GameType;
            RandomSong = info.RandomSong;
            NoteEffect = info.NoteEffect;
            FadeEffect = info.FadeEffect;
            Mode = info.Mode;

            Master = master;
            Channel = master.Channel;
            _clients[NewRoomMasterSlot] = master;
            master.Room = this;
            master.Player = new Player();
            master.Player.Slot = NewRoomMasterSlot;
            Playing = false;
            if (info.GameGroupType == GameGroupType.Team)
            {
                master.Player.Team = TeamType.Red;
            }
        }

        public RoomInfo GetInfo()
        {
            RoomInfo info = new RoomInfo();
            info.AllowViewer = AllowViewer;
            info.PasswordProtected = PasswordProtected;
            info.MaxPlayer = MaxPlayer;
            info.SelectedSong = SelectedSong;
            info.Name = Name;
            info.Password = Password;
            info.MaxDifficulty = MaxDifficulty;
            info.Difficulty = Difficulty;
            info.GameGroupType = GameGroupType;
            info.GameType = GameType;
            info.RandomSong = RandomSong;
            info.NoteEffect = NoteEffect;
            info.FadeEffect = FadeEffect;
            info.Mode = Mode;
            return info;
        }

        public void SetPlaying(bool playing)
        {
            lock (_lock)
            {
                Playing = playing;
            }
        }

        public void ChangeGameSetting(EzClient client, RoomOptionType roomOption, int valueA = 0, int valueB = 0)
        {
            lock (_lock)
            {
                if (Playing)
                {
                    // Block all changes after start game.
                    return;
                }

                switch (roomOption)
                {
                    case RoomOptionType.ChangeReady:
                    {
                        ReadyType ready = (ReadyType) valueA;
                        client.Player.Ready = ready;
                        break;
                    }
                    case RoomOptionType.ChangeTeam:
                    {
                        TeamType team = (TeamType) valueA;
                        client.Player.Team = team;
                        break;
                    }
                    case RoomOptionType.ChangeFade:
                        client.Room.FadeEffect = (FadeEffectType) valueA;
                        break;
                    case RoomOptionType.ChangeNote:
                        client.Room.NoteEffect = (NoteEffectType) valueA;
                        break;
                    case RoomOptionType.ChangeSongAndDifficulty:
                        client.Room.RandomSong = false;
                        client.Room.SelectedSong = valueA;
                        client.Room.Difficulty = (DifficultyType) valueB;
                        break;
                    case RoomOptionType.StartGame:
                        bool allReady = true;
                        Master.Player.Ready = ReadyType.Ready;
                        for (int i = 0; i < MaxSlots; i++)
                        {
                            if (_clients[i] != null
                                && _clients[i].Player.Ready == ReadyType.NotReady
                            )
                            {
                                allReady = false;
                                _logger.Debug(
                                    $"Room: [{Number}]{Name} Character: {_clients[i].Character.Name} not ready");
                                break;
                            }
                        }

                        if (allReady)
                        {
                            Playing = true;
                            _isStarting = true;
                            _trait.GameStarting(this);
                        }
                        else
                        {
                            Master.Player.Ready = ReadyType.NotReady;
                            valueA = 0;
                        }

                        break;
                    case RoomOptionType.ChangeRandom:
                        client.Room.RandomSong = true;
                        client.Room.SelectedSong = valueA;
                        client.Room.Difficulty = (DifficultyType) valueB;
                        break;
                    default:
                        _logger.Info(client, $"Unknown RoomOptionType: {roomOption} ValueA: {valueA} ValueB: {valueB}");
                        break;
                }
            }

            _trait.ChangeGameSetting(this, (short) client.Player.Slot, roomOption, valueA, valueB);
        }


        /// <summary>
        /// Returns the client specified by the index.
        /// </summary>
        public EzClient GetClient(int index)
        {
            if (index >= MaxSlots)
            {
                return null;
            }

            EzClient client;
            lock (_lock)
            {
                client = _clients[index];
            }

            return client;
        }

        /// <summary>
        /// Returns a list of all clients.
        /// </summary>
        public List<EzClient> GetClients()
        {
            List<EzClient> clients = new List<EzClient>();
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_clients[i] != null)
                    {
                        clients.Add(_clients[i]);
                    }
                }
            }

            return clients;
        }

        /// <summary>
        /// Closes this room, prevents joining and removes all clients.
        /// </summary>
        public void Close()
        {
            lock (_lock)
            {
                _isOpen = false;
            }

            List<EzClient> clients = GetClients();
            foreach (var client in clients)
            {
                Leave(client);
            }

            Channel.CloseRoom(this);
        }

        /// <summary>
        /// Try to join this room, returns true on success and false on failure.
        /// </summary>
        public bool Join(EzClient client)
        {
            bool success = false;

            if (client.Room != null)
            {
                client.Room.Leave(client);
            }

            lock (_lock)
            {
                if (!_isOpen || Playing)
                {
                    _logger.Debug(client, $"Tried to join a closed or playing Room: [{Number}]{Name}");
                    return false;
                }

                if (MaxPlayer <= GetClients().Count)
                {
                    _logger.Debug(client, $"Tried to join a full Room: [{Number}]{Name}");
                    return false;
                }

                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_clients[i] == null)
                    {
                        _clients[i] = client;
                        client.Room = this;
                        client.Player = new Player();
                        client.Player.Slot = i;
                        if (client.Room.GameGroupType == GameGroupType.Team)
                        {
                            client.Player.Team = GetTeam();
                        }

                        _logger.Debug(client, $"Joined Room: [{Number}]{Name}");
                        success = true;
                        break;
                    }
                }
            }

            if (success)
            {
                _trait.ClientJoin(this, client);
            }

            return success;
        }

        /// <summary>
        /// Kicks a player.
        /// </summary>
        /// <param name="client">Initiator of kicking</param>
        /// <param name="playerSlot">Slot to be kicked</param>
        public void Kick(EzClient client, byte playerSlot)
        {
            EzClient player;
            lock (_lock)
            {
                if (Master != client)
                {
                    _logger.Error(client, $"Character: {client.Character.Name} is not master");
                    return;
                }

                player = GetClient(playerSlot);
                if (player == null)
                {
                    _logger.Error("Player is null");
                    return;
                }

                _logger.Debug(client,
                    $"Character: {client.Character.Name} kicked Character: {player.Character.Name} from Room: [{Number}]{Name}");
            }

            _trait.Kick(this, client, playerSlot);
            Leave(player);
        }

        /// <summary>
        /// Removes a player from the room.
        /// </summary>
        public void Leave(EzClient client)
        {
            bool newMaster = false;
            bool leave = false;
            client.Room = null;

            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_clients[i] == client)
                    {
                        leave = true;
                        _clients[i] = null;
                        _logger.Debug(client, $"Left Room: [{Number}]{Name}");
                        break;
                    }
                }

                if (client == Master)
                {
                    Master = null;
                    for (int i = 0; i < MaxSlots; i++)
                    {
                        if (_clients[i] != null)
                        {
                            Master = _clients[i];
                            newMaster = true;
                            break;
                        }
                    }
                }

                if (Master == null)
                {
                    _isOpen = false;
                }

                if (_isStarting && leave)
                {
                    // If the game starts and only one player is left, the start need to be aborted.
                    int clientCount = 0;
                    for (int i = 0; i < MaxSlots; i++)
                    {
                        if (_clients[i] != null)
                        {
                            clientCount++;
                        }
                    }

                    if (clientCount == 1)
                    {
                        Playing = false;
                        _isStarting = false;
                    }
                }
            }

            if (leave)
            {
                _trait.ClientLeave(this, client);
            }

            if (newMaster)
            {
                _trait.NewMaster(this, Master);
            }

            if (Master == null)
            {
                Close();
            }

            client.Player = null;
        }

        /// <summary>
        /// Cancel game by pressing ESC
        /// </summary>
        public void Cancel(EzClient client)
        {
            _logger.Debug(client, $"Room: [{Number}]{Name} cancel game");
            client.Game = null;
            client.Rank = null;
            client.Score = null;
            client.BestScore = null;
            client.NoteHistory.Clear();
            client.Player.Ready = ReadyType.NotReady;
            client.Player.Playing = false;
            Game = null;
            lock (_lock)
            {
                Playing = false;
            }
        }

        /// <summary>
        /// Starts the game with the current room options and specified song.
        /// </summary>
        public void StartGame()
        {
            Radiomix radiomix = _server.GetRadiomix(SelectedSong);
            if (radiomix != null)
            {
                StartGame(radiomix);
                lock (_lock)
                {
                    _isStarting = false;
                }

                return;
            }

            Song song = _server.GetSong(SelectedSong);
            if (song != null)
            {
                StartGame(song);
                lock (_lock)
                {
                    _isStarting = false;
                }

                return;
            }

            _logger.Error("Could not find song");
            _trait.GameStart(this);
            lock (_lock)
            {
                _isStarting = false;
            }
        }


        public void StartGame(Song song)
        {
            if (!_server.IsRunning)
            {
                return;
            }

            lock (_lock)
            {
                Master.Player.Ready = ReadyType.Ready;
                Game = new Game(GetInfo(), song);
                _logger.Debug($"Room: [{Number}]{Name} starting Song: {song.Name}");

                foreach (EzClient c in GetClients())
                {
                    // Watcher will never submit a score and are not playing.
                    c.Player.Playing = c.Player.Ready == ReadyType.Ready;
                    c.Game = Game;

                    // Ensure all values are reset
                    c.Rank = null;
                    c.Score = null;
                    c.BestScore = null;

                    c.Player.Loading = true;
                }

                Playing = true;
            }

            _trait.GameStart(this);
        }

        public void StartGame(Radiomix radiomix)
        {
            if (!_server.IsRunning)
            {
                return;
            }

            RadiomixGame game = Game as RadiomixGame;
            if (game == null)
            {
                Song radiomixSong = _server.GetSong(radiomix.Id);
                lock (_lock)
                {
                    Master.Player.Ready = ReadyType.Ready;
                    Game = game = new RadiomixGame(radiomix, GetInfo(), radiomixSong);
                    _logger.Debug($"Room: [{Number}]{Name} starting Radiomix: {radiomixSong.Name}");
                    foreach (EzClient c in GetClients())
                    {
                        c.Game = Game;
                        c.Rank = null;
                        c.Score = null;
                        c.BestScore = null;
                    }

                    Song nextSong = _server.GetSong(game.CurrentSongId());
                    game.CurrentSong = nextSong;
                    Playing = true;
                }
            }

            foreach (EzClient c in GetClients())
            {
                // Watcher will never submit a score and are not playing.
                c.Player.Playing = c.Player.Ready == ReadyType.Ready;
                c.Player.Loading = true;
            }

            _logger.Info($"Room: [{Number}]{Name} Next Song: {game.CurrentSong.Name} for Radiomix: {Game.Song.Name}");
            _trait.GameStart(this);
        }

        public void FinishLoading(EzClient client)
        {
            client.Player.Loading = false;

            _logger.Debug(client, $"Finished loading in Room: [{Number}]{Name}");

            if (!IsLoadingFinished())
            {
                return;
            }

            _logger.Debug($"All clients ready in Room: [{Number}]{Name}");
            _trait.LoadingFinish(this);
        }

        /// <summary>
        /// Returns true if no one is playing.
        /// </summary>
        public bool IsGameFinished()
        {
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_clients[i] != null)
                    {
                        if (_clients[i].Player.Playing)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public bool IsLoadingFinished()
        {
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_clients[i] != null)
                    {
                        if (_clients[i].Player.Loading)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Notify that the current game finished.
        /// - If the room is empty it will be closed.
        /// - If everyone finished and at least one player with a valid score is present,
        ///   the result screen will be shown.
        /// </summary>
        public void FinishGame(EzClient finishedClient)
        {
            lock (_lock)
            {
                if (Game == null)
                {
                    _logger.Debug($"Room: [{Number}]{Name} game already finished");
                    return;
                }
            }

            if (finishedClient.Score != null)
            {
                _trait.ClientFinish(this, finishedClient);
            }

            if (finishedClient.Player != null)
            {
                finishedClient.Player.Playing = false;
            }

            lock (_lock)
            {
                if (!Playing)
                {
                    _logger.Debug($"Room: [{Number}]{Name} is not playing, can not finish");
                    return;
                }
            }

            if (Game is RadiomixGame rmGame)
            {
                if (rmGame.Index < 3)
                {
                    if (finishedClient.Player != null)
                    {
                        _trait.NextRadiomixSong(this, finishedClient);
                    }
                }
            }

            if (!IsGameFinished())
            {
                return;
            }

            List<EzClient> finished = GetClients();
            foreach (var client in finished)
            {
                Score score = client.Score;
                Player player = client.Player;
                if (player == null || (score == null && player.Ready == ReadyType.Ready))
                {
                    _logger.Debug(client, $"Left or no score, removing from Room: [{Number}]{Name}");
                    Leave(client);
                }
            }

            finished = GetClients();
            if (finished.Count <= 0)
            {
                _logger.Debug($"Room: [{Number}]{Name} has no players, closing");
                Close();
                return;
            }

            if (Game is RadiomixGame radiomixGame)
            {
                bool stageClear = false;
                foreach (EzClient client in finished)
                {
                    Score score = client.Score;
                    if (score != null && score.StageClear)
                    {
                        stageClear = true;
                    }
                }

                if (stageClear)
                {
                    radiomixGame.IncreaseIndex();
                    _logger.Info($"Room: [{Number}]{Name} Radiomix Increase Index: {radiomixGame.Index}");
                    if (radiomixGame.Index < 4)
                    {
                        Song nextSong = _server.GetSong(radiomixGame.CurrentSongId());
                        radiomixGame.CurrentSong = nextSong;
                        return;
                    }
                }
            }

            _trait.GameResult(this);

            foreach (var client in finished)
            {
                client.Game = null;
                client.Rank = null;
                client.Score = null;
                client.BestScore = null;
                client.NoteHistory.Clear();
                client.Player.Ready = ReadyType.NotReady;
                client.Player.Playing = false;
            }

            Game = null;

            Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith(t0 =>
            {
                // Display Room after 10 seconds
                _trait.GameFinish(this);
                Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith(t1 =>
                {
                    lock (_lock)
                    {
                        Playing = false;
                    }
                });
            });
        }

        public void ChangeRoomOptions()
        {
            lock (_lock)
            {
                if (GameGroupType == GameGroupType.Individual)
                {
                    foreach (EzClient client in GetClients())
                    {
                        if (client != null && client.Player != null)
                        {
                            client.Player.Team = TeamType.None;
                        }
                    }
                }
                else if (GameGroupType == GameGroupType.Team)
                {
                    int c = 0;
                    foreach (EzClient client in GetClients())
                    {
                        if (client != null && client.Player != null)
                        {
                            if (c % 2 == 0)
                            {
                                client.Player.Team = TeamType.Red;
                            }
                            else
                            {
                                client.Player.Team = TeamType.Blue;
                            }

                            c++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the team with fewer player.
        /// </summary>
        private TeamType GetTeam()
        {
            int red = 0;
            int blue = 0;
            foreach (EzClient client in GetClients())
            {
                if (client != null && client.Player != null)
                {
                    if (client.Player.Team == TeamType.Red)
                        red++;
                    else if (client.Player.Team == TeamType.Blue)
                        blue++;
                }
            }

            if (red > blue)
            {
                return TeamType.Blue;
            }

            return TeamType.Red;
        }
    }
}