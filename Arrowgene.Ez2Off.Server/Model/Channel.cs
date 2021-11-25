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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Logs;
using Arrowgene.Ez2Off.Server.Trait;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.Server.Model
{
    /// <summary>
    /// A thread safe channel.
    /// </summary>
    public class Channel
    {
        public const int MaxChannels = 10;
        public const int InvalidChannelIndex = -1;

        private const int StartRoomNumber = 1;
        private const int StartClientNumber = 0;
        private const int MaxRooms = 254;
        private const int MaxClients = 254;

        private readonly EzLogger _logger;
        private readonly object _lock;
        private readonly Room[] _rooms;
        private readonly EzClient[] _clients;
        private readonly ChannelTrait _channelTrait;
        private readonly RoomTrait _roomTrait;
        private readonly EzServer _server;

        public ChannelInfo Info { get; }

        public Channel(int id, ChannelTrait channelTrait, RoomTrait roomTrait, EzServer server)
        {
            _logger = LogProvider.Logger<EzLogger>(this);
            _channelTrait = channelTrait;
            _roomTrait = roomTrait;
            _server = server;
            _lock = new object();
            _rooms = new Room[MaxRooms];
            _clients = new EzClient[MaxClients];
            Info = new ChannelInfo {Id = id, Load = 0};
        }

        /// <summary>
        /// Join this channel, returns true on success and false on failure.
        /// </summary>
        public bool Join(EzClient client)
        {
            bool success = false;
            lock (_lock)
            {
                for (byte i = StartClientNumber; i < MaxClients; i++)
                {
                    if (_clients[i] == null)
                    {
                        Info.Load += 1;
                        client.Channel = this;
                        client.ChannelIndex = i;
                        _clients[i] = client;
                        _logger.Debug(client, $"Joined Channel: {Info.Id}");
                        success = true;
                        break;
                    }
                }
            }

            if (success)
            {
                _channelTrait.ClientJoin(this, client);
            }

            return success;
        }

        /// <summary>
        /// Removes a player from the channel.
        /// </summary>
        public void Leave(EzClient client)
        {
            lock (_lock)
            {
                Info.Load -= 1;
                _clients[client.ChannelIndex] = null;
                _logger.Debug(client, $"Left Channel: {Info.Id}");
            }

            client.Channel = null;
            _channelTrait.ClientLeave(this, client);
            client.ChannelIndex = InvalidChannelIndex;
        }

        /// <summary>
        /// Returns client by character name or null on failure.
        /// </summary>
        public EzClient GetClient(string characterName)
        {
            lock (_lock)
            {
                foreach (EzClient client in _clients)
                {
                    if (client != null && client.Character.Name == characterName)
                    {
                        return client;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a list of all clients in the lobby.
        /// Clients who are in a room are ignored.
        /// </summary>
        public List<EzClient> GetLobbyClients()
        {
            List<EzClient> clients = new List<EzClient>();
            lock (_lock)
            {
                foreach (EzClient client in _clients)
                {
                    if (client != null && client.Room == null)
                    {
                        clients.Add(client);
                    }
                }
            }

            return clients;
        }

        /// <summary>
        /// Returns all clients of the channel.
        /// Includes clients who are in a room.
        /// </summary>
        public List<EzClient> GetClients()
        {
            lock (_lock)
            {
                List<EzClient> clients = new List<EzClient>();
                foreach (EzClient client in _clients)
                {
                    if (client != null)
                    {
                        clients.Add(client);
                    }
                }

                return clients;
            }
        }

        /// <summary>
        /// Returns a new room or null on failure.
        /// </summary>
        public Room CreateRoom(RoomInfo roomInfo, EzClient master)
        {
            if (!_server.IsRunning)
            {
                return null;
            }

            Room room = null;

            lock (_lock)
            {
                for (byte i = StartRoomNumber; i < MaxRooms; i++)
                {
                    if (_rooms[i] == null)
                    {
                        room = new Room(i, roomInfo, master, _roomTrait, _server);
                        _rooms[i] = room;
                        _logger.Debug(master,
                            $"Created Room: [{room.Number}]{room.Name} in Channel: {Info.Id}");
                        break;
                    }
                }
            }

            if (room != null)
            {
                _channelTrait.CreateRoom(this, room, master);
            }

            return room;
        }

        /// <summary>
        /// Closes a room, prevents joining and removes all clients.
        /// </summary>
        public void CloseRoom(Room room)
        {
            if (room == null)
            {
                return;
            }

            bool closeRoom = false;

            lock (_lock)
            {
                if (_rooms[room.Number] == room)
                {
                    _rooms[room.Number] = null;
                    closeRoom = true;
                    _logger.Debug($"Closed Room: [{room.Number}]{room.Name} in Channel: {Info.Id}");
                }
            }

            if (closeRoom)
            {
                // Only close room if it is inside room list, to prevent recursion.
                room.Close();
            }

            _channelTrait.CloseRoom(this, room);
        }

        /// <summary>
        /// Returns a room by its id or null if it does not exist.
        /// </summary>
        public Room GetRoom(short id)
        {
            lock (_lock)
            {
                return _rooms[id];
            }
        }


        /// <summary>
        /// Returns all rooms of this channel.
        /// </summary>
        public List<Room> GetRooms()
        {
            List<Room> rooms = new List<Room>();
            lock (_lock)
            {
                for (int i = StartRoomNumber; i < MaxRooms; i++)
                {
                    if (_rooms[i] != null)
                    {
                        rooms.Add(_rooms[i]);
                    }
                }
            }

            return rooms;
        }

        /// <summary>
        /// Returns a room that is not playing, has no password and a free slot.
        /// If none is available null is returned.
        /// </summary>
        public Room GetQuickRoom()
        {
            List<Room> rooms = GetRooms();
            foreach (Room room in rooms)
            {
                if (!room.PasswordProtected
                    && !room.Playing
                    && room.GetClients().Count < room.MaxPlayer)
                {
                    return room;
                }
            }

            return null;
        }
    }
}