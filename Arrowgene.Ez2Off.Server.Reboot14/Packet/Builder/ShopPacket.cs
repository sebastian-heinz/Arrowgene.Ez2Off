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
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Packet.Builder;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder
{
    public class ShopPacket : IShopPacket
    {
        public IBuffer CreatePurchasePacket(Item item, Character character, ShopPurchaseItemMessageType messageType)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32((byte) messageType); /* 3=slot full 2=haha...fucking korea law */
            buffer.WriteByte(0);
            if (item == null)
            {
                buffer.WriteInt32(0);
            }
            else
            {
                buffer.WriteInt32((short) item.Id, Endianness.Big);
            }

            buffer.WriteInt32(character.Coin, Endianness.Big);
            buffer.WriteInt32(character.Cash, Endianness.Big);
            return buffer;
        }

        public EzPacket CreateNewGiftNotification()
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32(1);
            return new EzPacket(47, buffer);
        }

        public EzPacket CreateSendGift(Character sender, GiftItem gift, string receiverCharacterName)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32((int) SendGiftResponseType.Success);

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteInt16((short) gift.ItemId);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteFixedString(receiverCharacterName, 18, Utils.KoreanEncoding);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteInt32(sender.Coin);
            buffer.WriteInt32(sender.Cash);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            return new EzPacket(48, buffer);
        }

        public EzPacket CreateSendGift(SendGiftResponseType responseType)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32((int) responseType);
            return new EzPacket(48, buffer);
        }
    }
}