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

using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Logs;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Sessions;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.Server.Packet
{
    public abstract class Handler<T> : IHandler where T : EzServer
    {
        protected Handler(T server)
        {
            Logger = LogProvider.Logger<EzLogger>(this);
            Server = server;
            Settings = Server.Settings;
            Sessions = Server.Sessions;
            Database = Server.Database;
            Router = Server.Router;
        }

        public abstract int Id { get; }
        public virtual int ExpectedSize => EzServer.NoExpectedSize;
        protected T Server { get; }
        protected EzSettings Settings { get; }
        protected SessionManager Sessions { get; }
        protected IDatabase Database { get; }
        protected EzLogger Logger { get; }
        protected PacketRouter Router { get; }
        public abstract void Handle(EzClient client, EzPacket packet);
    }
}