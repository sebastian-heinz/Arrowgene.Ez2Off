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
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.Login
{
    public class SelectMode : Handler<EzServer>
    {
        public SelectMode(EzServer server) : base(server)
        {
        }

        public override int Id => 11;

        public override void Handle(EzClient client, EzPacket packet)
        {
            ModeType mode = (ModeType) packet.Data.ReadByte();
            Logger.Debug(client, $"Selected Mode: {mode}");
            client.Session.Mode = mode;
            IBuffer response = EzServer.Buffer.Provide();
            List<ServerPoint> worldServers = Server.GetServerPoints();
            response.WriteByte(0);
            response.WriteByte((byte) worldServers.Count);
            foreach (ServerPoint worldServer in worldServers)
            {
                response.WriteByte((byte) worldServer.Id);
                response.WriteInt16(CalculateLoad(worldServer.GetLoad(mode), Settings), Endianness.Big);
            }

            Router.Send(client, 10, response);
        }

        public static short CalculateLoad(short load, EzSettings settings)
        {
            int result = load * settings.ServerLoadMultiplier;
            if (settings.CombineChannel)
            {
                int multiplier = ServerPoint.MaxLoad / ChannelInfo.MaxLoad;
                result *= multiplier;
            }

            if (result > ServerPoint.MaxLoad)
            {
                result = ServerPoint.MaxLoad;
            }

            return (short) result;
        }
    }
}