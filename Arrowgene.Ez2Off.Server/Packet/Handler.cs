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
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Log;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Sessions;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.ServerBridge;

namespace Arrowgene.Ez2Off.Server.Packet
{
    public abstract class Handler<T> : IHandler where T : EzServer
    {
        protected T Server { get; }
        protected EzServerSettings Settings { get; }
        protected ISessionManager SessionManager { get; }
        protected IDatabase Database { get; }
        protected IBridge Bridge { get; }

        // TODO rename
        protected EzLogger _logger { get; }

        protected Handler(T server)
        {
            _logger = LogProvider<EzLogger>.GetLogger(this);
            Server = server;
            Settings = Server.Settings;
            SessionManager = Server.SessionManager;
            Database = Server.Database;
            Bridge = Server.Bridge;
        }

        public abstract int Id { get; }

        public abstract void Handle(EzClient client, EzPacket packet);


        protected void Send(EzClient client, EzPacket packet)
        {
            Server.Send(client, packet);
        }

        protected void Send(List<EzClient> clients, EzPacket packet)
        {
            Server.Send(clients, packet);
        }

        protected void Send(EzClient client, byte id, IBuffer data)
        {
            Server.Send(client, id, data);
        }

        protected void Send(List<EzClient> clients, byte id, IBuffer data)
        {
            Server.Send(clients, id, data);
        }

        protected void Send(Channel channel, byte id, IBuffer data)
        {
            Server.Send(channel, id, data);
        }

        protected void Send(Room room, byte id, IBuffer data)
        {
            Server.Send(room, id, data);
        }
    }
}