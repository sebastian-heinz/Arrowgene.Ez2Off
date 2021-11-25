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

using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder
{
    public class ShopPacket
    {
        public static IBuffer CreatePurchasePacket(Item item, Character character)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(1);
            buffer.WriteByte(0); //MY ITEM Slot
            buffer.WriteInt16((short) item.Id, Endianness.Big); //ItmeCode 1~5
            buffer.WriteByte(0);
            buffer.WriteByte(1);

            buffer.WriteInt32(character.Coin, Endianness.Big);
            buffer.WriteInt32(character.Cash, Endianness.Big);
            // buffer.WriteByte(0);
            // buffer.WriteByte(0);
            // buffer.WriteByte(0X27); //Coin // ??? buffer.WriteInt16((short)item.Price); ???
            // buffer.WriteByte(0X10);
            // buffer.WriteByte(0);
            // buffer.WriteByte(0);
            // buffer.WriteByte(0X27); //Cash
            // buffer.WriteByte(0X10);
            return buffer;
        }
    }
}