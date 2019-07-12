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
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Services.Networking.ServerBridge;
using Arrowgene.Services.Networking.ServerBridge.Messages;

namespace Arrowgene.Ez2Off.Server.Bridge
{
    public class ChannelInfoHandler : IMessageHandler<ModeType, List<ChannelInfo>>
    {
        public const string Id = "ChannelInfoHandler";

        private EzWorldServer _worldServer;

        public ChannelInfoHandler(EzWorldServer worldServer)
        {
            _worldServer = worldServer;
        }

        public Response<List<ChannelInfo>> Handle(Request<ModeType> request)
        {
            List<ChannelInfo> channelInfoList = new List<ChannelInfo>();
            Channel[] channels = _worldServer.GetChannels(request.Content);
            foreach (Channel channel in channels)
            {
                channelInfoList.Add(channel.Info);
            }

            return new Response<List<ChannelInfo>>(request, channelInfoList);
        }

        public string HandlerId => Id;
    }
}