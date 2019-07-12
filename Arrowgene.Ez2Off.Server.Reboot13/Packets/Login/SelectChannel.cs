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

using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.Login
{
    public class SelectChannel : Handler<LoginServer>
    {
        public SelectChannel(LoginServer server) : base(server)
        {
        }

        public override int Id => 6;

        public override void Handle(EzClient client, EzPacket packet)
        {
            packet.Data.ReadByte();
            int selectedChannel = packet.Data.ReadByte();
            _logger.Debug("Selected Channel: {0}", selectedChannel + 1);
            client.Session.ChannelId = selectedChannel;
           // Channel channel = Server.GetChannel(client.Mode, selectedChannel);
            IBuffer response = EzServer.Buffer.Provide();
            response.WriteInt16((short) client.WorldServer.Public.Port, Endianness.Big); //World Server Port
            response.WriteString(client.WorldServer.Public.DataPublicIpAddress); // World Server Ip
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0x07);
            Send(client, 7, response);
        }
    }
}