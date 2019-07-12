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
using Arrowgene.Services.Networking.Tcp;

namespace Arrowgene.Ez2Off.Server.Client
{
    public class ClientLookup
    {
        private Dictionary<ITcpSocket, EzClient> _clients;
        private readonly object _lock = new object();

        public ClientLookup()
        {
            _clients = new Dictionary<ITcpSocket, EzClient>();
        }

        public List<EzClient> GetAllClients()
        {
            List<EzClient> clients;
            lock (_lock)
            {
                clients = new List<EzClient>(_clients.Values);
            }
            return clients;
        }

        public void AddClient(EzClient client)
        {
            lock (_lock)
            {
                if (!_clients.ContainsKey(client.Socket))
                    _clients.Add(client.Socket, client);
            }
        }

        public void RemoveClient(ITcpSocket socket)
        {
            lock (_lock)
            {
                if (_clients.ContainsKey(socket))
                    _clients.Remove(socket);
            }
        }

        public void RemoveClient(EzClient client)
        {
            lock (_lock)
            {
                if (_clients.ContainsKey(client.Socket))
                    _clients.Remove(client.Socket);
            }
        }

        public EzClient GetClient(ITcpSocket socket)
        {
            EzClient client = null;
            lock (_lock)
            {
                if (_clients.ContainsKey(socket))
                    client = _clients[socket];
            }
            return client;
        }

        public bool Contains(ITcpSocket id)
        {
            bool contains;
            lock (_lock)
            {
                if (_clients.ContainsKey(id))
                    contains = true;
                else
                    contains = false;
            }
            return contains;
        }

        public int Count()
        {
            int count;
            lock (_lock)
            {
                count = _clients.Count;
            }
            return count;
        }
    }
}