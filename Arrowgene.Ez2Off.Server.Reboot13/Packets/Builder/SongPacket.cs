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

using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder
{
    public class SongPacket
    {
        public static IBuffer CreateDjPointsPacket()
        {
            //Song DJPoint
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);


            for (int i = 0; i < 219; i++)
            {
                buffer.WriteInt16(99); //Disc Num 1
                buffer.WriteInt32(0); //AII DJPoint

                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }

            return buffer;
        }
    }
}