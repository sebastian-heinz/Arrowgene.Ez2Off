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
using Arrowgene.Ez2Off.Server.Logs;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.Server.Packet
{
    public class PacketRouter
    {
        private readonly EzLogger _logger;

        public PacketRouter()
        {
            _logger = LogProvider.Logger<EzLogger>(this);
        }

        /// <summary>
        /// Send a packet to a client.
        /// </summary>
        public void Send(EzClient client, EzPacket packet)
        {
            client.Send(packet);
        }

        /// <summary>
        /// Send a packet to a client.
        /// </summary>
        public void Send(EzClient client, byte id, IBuffer data)
        {
            EzPacket packet = new EzPacket(id, data);
            Send(client, packet);
        }

        /// <summary>
        /// Send a packet to multiple clients.
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void Send(List<EzClient> clients, EzPacket packet, params EzClient[] excepts)
        {
            clients = GetClients(clients, excepts);
            foreach (EzClient client in clients)
            {
                Send(client, packet);
            }
        }

        /// <summary>
        /// Send a packet to multiple clients.
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void Send(List<EzClient> clients, byte id, IBuffer data, params EzClient[] excepts)
        {
            clients = GetClients(clients, excepts);
            foreach (EzClient client in clients)
            {
                Send(client, id, data);
            }
        }

        /// <summary>
        /// Send a packet to all clients in a channel. (including clients inside rooms)
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void Send(Channel channel, byte id, IBuffer data, params EzClient[] excepts)
        {
            Send(channel.GetClients(), id, data, excepts);
        }

        /// <summary>
        /// Send a packet to all clients in a channel lobby. (excluding clients inside rooms)
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void SendLobby(Channel channel, byte id, IBuffer data, params EzClient[] excepts)
        {
            Send(channel.GetLobbyClients(), id, data, excepts);
        }

        /// <summary>
        /// Send a packet to all clients in a room.
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void Send(Room room, byte id, IBuffer data, params EzClient[] excepts)
        {
            Send(room.GetClients(), id, data, excepts);
        }

        private List<EzClient> GetClients(List<EzClient> clients, params EzClient[] excepts)
        {
            if (excepts.Length == 0)
            {
                return clients;
            }

            foreach (EzClient except in excepts)
            {
                clients.Remove(except);
            }

            return clients;
        }
    }
}