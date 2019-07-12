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
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Services.Buffers;
using System.Collections.Generic;
using System.Net;
using Arrowgene.Ez2Off.Server.Bridge;
using Arrowgene.Services.Networking.ServerBridge.Messages;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.Login
{
    public class SelectServer : Handler<LoginServer>
    {
        public SelectServer(LoginServer server) : base(server)
        {
        }

        public override int Id => 7;

        public override void Handle(EzClient client, EzPacket packet)
        {
            packet.Data.ReadByte();
            int selectedServerId = packet.Data.ReadByte();
            _logger.Debug("Selected Server: {0}", selectedServerId + 1);

            ServerPoint selectedServer = Server.GetServerPoint(selectedServerId);
            client.WorldServer = selectedServer;

            Bridge.Request<List<ChannelInfo>, EzClient, EzPacket>(selectedServer.Bridge.ToIpEndPoint(),
                new Request<ModeType>(ChannelInfoHandler.Id, client.Mode), OnChannelInfoResponse, client, packet);
        }

        private void OnChannelInfoResponse(Response<List<ChannelInfo>> result, EzClient client, EzPacket packet)
        {
            List<ChannelInfo> channelInfoList = result.Result;
            IBuffer response1 = EzServer.Buffer.Provide();
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte((byte) channelInfoList.Count);
            foreach (ChannelInfo channelInfo in channelInfoList)
            {
                response1.WriteInt16(channelInfo.Load, Endianness.Big);
            }

            Send(client, 0x08, response1);
        }
    }
}