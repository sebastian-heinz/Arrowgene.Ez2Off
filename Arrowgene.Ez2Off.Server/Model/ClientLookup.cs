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

namespace Arrowgene.Ez2Off.Server.Model
{
    public class ClientLookup
    {
        private readonly List<EzClient> _clients;

        private readonly object _lock = new object();

        public ClientLookup()
        {
            _clients = new List<EzClient>();
        }

        /// <summary>
        /// Returns all Clients.
        /// </summary>
        public List<EzClient> GetAllClients()
        {
            lock (_lock)
            {
                return new List<EzClient>(_clients);
            }
        }

        /// <summary>
        /// Adds a Client.
        /// </summary>
        public void AddClient(EzClient client)
        {
            if (client == null)
            {
                return;
            }

            lock (_lock)
            {
                _clients.Add(client);
            }
        }

        /// <summary>
        /// Removes the Client from all lists and lookup tabes.
        /// </summary>
        public void RemoveClient(EzClient client)
        {
            lock (_lock)
            {
                _clients.Remove(client);
            }
        }

        /// <summary>
        /// Returns a Client by CharacterName if it exists.
        /// </summary>
        public EzClient GetClient(string characterName)
        {
            List<EzClient> clients = GetAllClients();
            foreach (EzClient client in clients)
            {
                if (client.Character.Name == characterName)
                {
                    return client;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a Client by CharacterName if it exists.
        /// </summary>
        public EzClient GetClient(int characterId)
        {
            List<EzClient> clients = GetAllClients();
            foreach (EzClient client in clients)
            {
                if (client.Character.Id == characterId)
                {
                    return client;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a Client by AccountId if it exists.
        /// </summary>
        public EzClient GetClientByAccountId(int accountId)
        {
            List<EzClient> clients = GetAllClients();
            foreach (EzClient client in clients)
            {
                if (client.Account.Id == accountId)
                {
                    return client;
                }
            }

            return null;
        }

        /// <summary>
        /// List of clients who have added me as a friend.
        /// </summary>
        public List<EzClient> GetFriendedMe(EzClient client)
        {
            List<EzClient> onlineFriends = new List<EzClient>();
            List<EzClient> onlineClients = GetAllClients();
            foreach (EzClient onlineClient in onlineClients)
            {
                Friend friend = onlineClient.Friends.Get(client.Character.Id);
                if (friend != null)
                {
                    onlineFriends.Add(onlineClient);
                }
            }

            return onlineFriends;
        }
    }
}