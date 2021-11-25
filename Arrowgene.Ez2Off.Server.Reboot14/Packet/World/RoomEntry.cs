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

using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Id;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class RoomEntry : Handler<EzServer>
    {
        public RoomEntry(EzServer server) : base(server)
        {
        }

        public override int Id => (int) WorldRequestId.RoomEntry;

        public override void Handle(EzClient client, EzPacket packet)
        {
            if (client.Room != null)
            {
                client.Room.Leave(client);
            }

            byte quickStart = packet.Data.ReadByte();
            byte passwordProtected = packet.Data.ReadByte();
            byte unknown0 = packet.Data.ReadByte();
            byte channel = packet.Data.ReadByte(); // Channel + 100, 0 Indexed
            byte unknown1 = packet.Data.ReadByte();
            byte unknown2 = packet.Data.ReadByte();
            byte roomNumber = packet.Data.ReadByte();
            string password = packet.Data.ReadFixedString(4, Utils.KoreanEncoding);

            byte channelNum = (byte) (channel - 100 + 1);

            Logger.Debug(client,
                $"Channel: {channelNum} Password: {password} Room#:{roomNumber} QuickStart: {quickStart}, PwProtected: {passwordProtected}");

            Room room;
            if (quickStart > 0)
            {
                room = client.Channel.GetQuickRoom();
                if (room == null)
                {
                    Logger.Debug(client, "No available QuickStart Room");
                    IBuffer joinErrorPacket = PacketBuilder.RoomPacket.CreateJoinErrorPacket();
                    Router.Send(client, 8, joinErrorPacket);
                    return;
                }
            }
            else
            {
                room = client.Channel.GetRoom(roomNumber);
            }

            if (room == null)
            {
                Logger.Error(client, "Invalid room");
                IBuffer joinErrorPacket = PacketBuilder.RoomPacket.CreateJoinErrorPacket();
                Router.Send(client, 8, joinErrorPacket);
                return;
            }

            if (passwordProtected > 0 && room.Password != password)
            {
                Logger.Error(client, $"Invalid Password: {password} for room with Password: {room.Password}");
                IBuffer joinErrorPacket = PacketBuilder.RoomPacket.CreateJoinErrorPacket();
                Router.Send(client, 8, joinErrorPacket);
                return;
            }

            room.Join(client);
        }
    }
}