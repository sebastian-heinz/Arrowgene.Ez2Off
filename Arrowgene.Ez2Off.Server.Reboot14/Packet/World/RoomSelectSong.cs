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

using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class RoomSelectSong : Handler<EzServer>
    {
        public RoomSelectSong(EzServer server) : base(server)
        {
        }

        public override int Id => 9;

        public override void Handle(EzClient client, EzPacket packet)
        {
            RoomOptionType roomOption = (RoomOptionType) packet.Data.ReadInt32();
            int valueA = packet.Data.ReadInt32();
            int valueB = packet.Data.ReadInt32();
            client.Room.ChangeGameSetting(client, roomOption, valueA, valueB);
        }
    }
}