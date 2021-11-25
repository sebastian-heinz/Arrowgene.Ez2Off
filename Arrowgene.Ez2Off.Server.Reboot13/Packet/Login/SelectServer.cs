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
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.Login
{
    public class SelectServer : Handler<EzServer>
    {
        public SelectServer(EzServer server) : base(server)
        {
        }

        public override int Id => 7;

        public override void Handle(EzClient client, EzPacket packet)
        {
            packet.Data.ReadByte();
            int selectedServerId = packet.Data.ReadByte();
            Logger.Debug(client, $"Selected Server: {selectedServerId + 1}");
            client.Session.ServerId = selectedServerId;

            List<ChannelInfo> channelInfoList = new List<ChannelInfo>();
            Channel[] channels = Server.GetChannels(client.Mode);
            foreach (Channel channel in channels)
            {
                channelInfoList.Add(channel.Info);
            }

            IBuffer response1 = EzServer.Buffer.Provide();
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte((byte) channelInfoList.Count);
            foreach (ChannelInfo channelInfo in channelInfoList)
            {
                response1.WriteInt16(CalculateLoad(channelInfo.Load, channelInfoList[0].Load, Settings),
                    Endianness.Big);
            }

            Router.Send(client, 0x08, response1);
        }

        public static short CalculateLoad(short load, short combinedLoad, EzSettings settings)
        {
            int result = load;

            if (settings.CombineChannel)
            {
                result = combinedLoad;
            }

            result = result * settings.ChannelLoadMultiplier;
            if (result > ChannelInfo.MaxLoad)
            {
                result = ChannelInfo.MaxLoad;
            }

            return (short) result;
        }
    }
}