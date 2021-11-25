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

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class ChangeChannel : Handler<EzServer>
    {
        public ChangeChannel(EzServer server) : base(server)
        {
        }

        public override int Id => 3; //로비에서 채널 선택

        
        public override void Handle(EzClient client, EzPacket packet)
        {
            List<ChannelInfo> channelInfoList = new List<ChannelInfo>();
            Channel[] channels = Server.GetChannels(client.Mode);
            foreach (Channel channel in channels)
            {
                channelInfoList.Add(channel.Info);
            }

            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte((byte) channelInfoList.Count);
   
            foreach (ChannelInfo channelInfo in channelInfoList)
            {
                buffer.WriteInt16(CalculateLoad(channelInfo.Load, channelInfoList[0].Load, Settings),
                    Endianness.Big);
            }

            Router.Send(client, 0, buffer);
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