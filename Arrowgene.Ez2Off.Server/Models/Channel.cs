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

namespace Arrowgene.Ez2Off.Server.Models
{
    public class Channel
    {
        public const int MaxRooms = 254;

        private object _roomLock;
        private object _clientLock;
        private Room[] _rooms;
        private List<EzClient> _clients;

        public Channel(int id)
        {
            _roomLock = new object();
            _clientLock = new object();
            _rooms = new Room[MaxRooms];
            _clients = new List<EzClient>();
            Info = new ChannelInfo();
            Info.Id = id;
            Info.Load = 0;
        }

        public ChannelInfo Info { get; }

        public void Join(EzClient client)
        {
            client.Channel = this;
            lock (_clientLock)
            {
                _clients.Add(client);
            }
        }

        public void Leave(EzClient client)
        {
            client.Channel = null;
            lock (_clientLock)
            {
                _clients.Remove(client);
            }
        }

        public EzClient GetClient(string characterName)
        {
            lock (_clientLock)
            {
                foreach (EzClient client in _clients)
                {
                    if (client.Character.Name == characterName)
                    {
                        return client;
                    }
                }
            }

            return null;
        }

        public List<EzClient> GetLobbyClients()
        {
            List<EzClient> clients = new List<EzClient>();
            lock (_clientLock)
            {
                foreach (EzClient client in _clients)
                {
                    if (client.Room == null)
                    {
                        clients.Add(client);
                    }
                }
            }

            return clients;
        }

        public List<EzClient> GetClients()
        {
            lock (_clientLock)
            {
                return new List<EzClient>(_clients);
            }
        }

        public Room CreateRoom(RoomInfo roomInfo, EzClient master)
        {
            lock (_roomLock)
            {
                for (byte i = 0; i < MaxRooms; i++)
                {
                    if (_rooms[i] == null)
                    {
                        Room room = new Room(i, roomInfo, master);
                        _rooms[i] = room;
                        return room;
                    }
                }

                return null;
            }
        }

        public void CloseRoom(Room room)
        {
            lock (_roomLock)
            {
                _rooms[room.Info.Number] = null;
            }
        }

        public Room GetRoom(short id)
        {
            lock (_roomLock)
            {
                return _rooms[id];
            }
        }

        public Room GetQuickRoom()
        {
            lock (_roomLock)
            {
                for (int i = 0; i < MaxRooms; i++)
                {
                    if (_rooms[i] != null)
                    {
                        Room room = _rooms[i];
                        if (!room.Info.PasswordProtected 
                            && room.GetClients().Count < room.Info.MaxPlayer)
                        {
                            // Not Password Protected
                            // Free slow
                            return room;
                        }
                    }
                }
            }
            return null;
        }

        public List<Room> GetRooms()
        {
            List<Room> rooms = new List<Room>();
            lock (_roomLock)
            {
                for (int i = 0; i < MaxRooms; i++)
                {
                    if (_rooms[i] != null)
                    {
                        rooms.Add(_rooms[i]);
                    }
                }
            }

            return rooms;
        }
    }
}