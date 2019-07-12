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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Services.Logging;

namespace Arrowgene.Ez2Off.Server.Models
{
    public class Room
    {
        public const int MaxSlots = 8;

        private readonly EzClient[] _clients;
        private readonly object _lock;
        private readonly Channel _channel;

        public Room(byte id, RoomInfo info, EzClient master)
        {
            _lock = new object();
            _clients = new EzClient[MaxSlots];
            Info = info;
            Info.Number = id;
            Master = master;
            _channel = master.Channel;
            Join(master);
        }

        public RoomInfo Info { get; }
        public EzClient Master { get; private set; }

        public void Join(EzClient client)
        {
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_clients[i] == null)
                    {
                        _clients[i] = client;
                        client.Room = this;
                        client.Player = new Player();
                        client.Player.Slot = i;
                        break;
                    }
                }
            }
        }

        public void Leave(EzClient client)
        {
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_clients[i] == client)
                    {
                        _clients[i] = null;
                    }
                }

                if (client == Master)
                {
                    // Master left the room, find a new one.
                    Master = null;
                    for (int i = 0; i < MaxSlots; i++)
                    {
                        if (_clients[i] != null)
                        {
                            Master = _clients[i];
                            // TODO announce new master.
                        }
                    }
                }

                client.Room = null;

                if (Master == null)
                {
                    // No people inside the room, close the room.
                    _channel.CloseRoom(this);
                }
            }
        }

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

        public void Log(Logger logger)
        {
            logger.Debug("Name: {0}", Info.Name);
            logger.Debug("Number: {0}", Info.Number);
            logger.Debug("Password Protected: {0}", Info.PasswordProtected);
            logger.Debug("Password: {0}", Info.Password);
            logger.Debug("GameType: {0}", Info.GameType);
            logger.Debug("GameGroupType: {0}", Info.GameGroupType);
            logger.Debug("Allow Viewer: {0}", Info.AllowViewer);
            logger.Debug("Max Difficulty: {0}", Info.MaxDifficulty);
            logger.Debug("Max Player: {0}", Info.MaxPlayer);
            logger.Debug("SelectedSong: {0}", Info.SelectedSong);
            logger.Debug("SelectedDifficulty: {0}", Info.Difficulty);
            logger.Debug("RandomSong: {0}", Info.RandomSong);
            logger.Debug("NoteEffect: {0}", Info.NoteEffect);
            logger.Debug("FadeEffect: {0}", Info.FadeEffect);
            logger.Debug("Playing: {0}", Info.Playing);
        }

        public bool Finished()
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
    }
}