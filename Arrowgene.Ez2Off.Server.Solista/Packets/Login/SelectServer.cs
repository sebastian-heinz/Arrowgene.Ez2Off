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
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Solista.Packets.Login
{
    public class SelectServer : Handler<LoginServer>
    {
        public SelectServer(LoginServer server) : base(server)
        {
        }

        public override int Id => 6;

        public override void Handle(EzClient client, EzPacket packet)
        {
            packet.Data.ReadByte();
            int selectedServer = packet.Data.ReadByte();
            _logger.Debug("Selected EzServer: {0}", selectedServer + 1); //Zero Indexed

            List<ServerPoint> worldServers = Server.WorldServerPoints;

            IBuffer response = EzServer.Buffer.Provide();
            response.WriteInt16((short) worldServers[0].Public.Port, Endianness.Big);
            response.WriteString(worldServers[0].Public.DataPublicIpAddress);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0x7);
            Send(client, 7, response);
        }
    }
}