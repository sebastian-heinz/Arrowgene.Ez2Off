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
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class LobbyEnter : Handler<EzServer>
    {
        public LobbyEnter(EzServer server) : base(server)
        {
        }

        public override int Id => 1;

        public override void Handle(EzClient client, EzPacket packet)
        {
            string sessionKey = packet.Data.ReadFixedString(16);

            Logger.Debug(client, $"Connected to GameServer with SessionKey: {sessionKey})");

            Session session = Server.Sessions.GetSession(sessionKey);

            if (session == null)
            {
                Logger.Error(client, "Session Result is null");
                client.Socket.Close();
                return;
            }

            Server.GameServer_OnClientAuthenticated(client, session);
            Server.Sessions.DeleteSession(session.Key);
            Channel channel = Server.GetChannel(client.Mode, client.Session.ChannelId);
            channel.Join(client);
        }
    }
}