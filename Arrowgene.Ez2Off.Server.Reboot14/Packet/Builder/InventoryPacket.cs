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
    public class InventoryPacket : IInventoryPacket
    {
        public EzPacket ShowGiftsPacket(GiftItem[] gifts)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32(Inventory.MaxGifts);
            for (int i = 0; i < Inventory.MaxGifts; i++)
            {
                GiftItem gift = gifts[i];
                if (gift == null)
                {
                    buffer.WriteInt32(i);
                    buffer.WriteBytes(new byte[44]);
                }
                else
                {
                    buffer.WriteInt32(i);
                    buffer.WriteFixedString(gift.SenderName, 18, Utils.KoreanEncoding);
                    buffer.WriteInt16(0);
                    buffer.WriteInt32(0);
                    buffer.WriteInt16(0);
                    buffer.WriteInt32((short) gift.ItemId);
                    buffer.WriteInt32(0);
                    buffer.WriteInt32(0);
                    buffer.WriteInt16(0);
                    buffer.WriteInt32(gift.GetExpireDateUnixTime());
                }
            }

            return new EzPacket(49, buffer);
        }

        public IBuffer ShowInventoryPacket(Inventory inventory)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            WriteEquipSlots(inventory, buffer);
            WritePremiumSlot(inventory, buffer);
            for (int i = 0; i < Inventory.MaxItems; i++)
            {
                WriteInventorySlot(i, inventory, buffer);
            }

            return buffer;
        }


        public void WriteEquipSlots(Inventory inventory, IBuffer buffer)
        {
            WriteEquipSlot(0, inventory, buffer);
            WriteEquipSlot(1, inventory, buffer);
            WriteEquipSlot(2, inventory, buffer);
        }

        public void WriteEquipSlot(int slot, Inventory inventory, IBuffer buffer)
        {
            InventoryItem item = inventory.GetEquip(slot);
            if (item == null)
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteInt32(0); // Item ID
                buffer.WriteInt32(0); // Equip Date
                buffer.WriteByte(0); // remaining period 1일
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteInt32(item.Item.Id);
                buffer.WriteInt32(item.GetEquipDateUnixTime());
                buffer.WriteInt16((short) item.UsedDayCount());
            }
        }

        public void WritePremiumSlot(Inventory inventory, IBuffer buffer)
        {
            InventoryItem premium1 = inventory.GetEquip(3);
            InventoryItem premium2 = inventory.GetEquip(4);
            InventoryItem premium3 = inventory.GetEquip(5);
            InventoryItem premium4 = inventory.GetEquip(6);

            if (premium1 != null)
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }

            if (premium2 != null)
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }

            if (premium3 != null)
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }

            if (premium4 != null)
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }


            if (premium1 != null)
            {
                buffer.WriteInt32(premium1.Item.Id);
            }
            else
            {
                buffer.WriteInt32(0);
            }

            if (premium2 != null)
            {
                buffer.WriteInt32(premium2.Item.Id);
            }
            else
            {
                buffer.WriteInt32(0);
            }

            if (premium3 != null)
            {
                buffer.WriteInt32(premium3.Item.Id);
            }
            else
            {
                buffer.WriteInt32(0);
            }

            if (premium4 != null)
            {
                buffer.WriteInt32(premium4.Item.Id);
            }
            else
            {
                buffer.WriteInt32(0);
            }


            if (premium1 != null)
            {
                buffer.WriteInt32(premium1.GetEquipDateUnixTime());
            }
            else
            {
                buffer.WriteInt32(0);
            }

            if (premium2 != null)
            {
                buffer.WriteInt32(premium2.GetEquipDateUnixTime());
            }
            else
            {
                buffer.WriteInt32(0);
            }

            if (premium3 != null)
            {
                buffer.WriteInt32(premium3.GetEquipDateUnixTime());
            }
            else
            {
                buffer.WriteInt32(0);
            }

            if (premium4 != null)
            {
                buffer.WriteInt32(premium4.GetEquipDateUnixTime());
            }
            else
            {
                buffer.WriteInt32(0);
            }


            if (premium1 != null)
            {
                buffer.WriteInt16((short) premium1.UsedDayCount());
            }
            else
            {
                buffer.WriteInt16(0); //remaining period 1일
            }

            if (premium2 != null)
            {
                buffer.WriteInt16((short) premium2.UsedDayCount());
            }
            else
            {
                buffer.WriteInt16(0); //remaining period 1일
            }

            if (premium3 != null)
            {
                buffer.WriteInt16((short) premium3.UsedDayCount());
            }
            else
            {
                buffer.WriteInt16(0); //remaining period 1일
            }

            if (premium4 != null)
            {
                buffer.WriteInt16((short) premium4.UsedDayCount());
            }
            else
            {
                buffer.WriteInt16(0); //remaining period 1일
            }
        }

        public void WriteInventorySlot(int slot, Inventory inventory, IBuffer buffer)
        {
            InventoryItem item = inventory.GetItem(slot);
            if (item != null)
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteInt32(item.Item.Id);
                buffer.WriteInt32(item.GetEquipDateUnixTime());
                buffer.WriteInt16((short) item.UsedDayCount());
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteInt32(0); // Item Id
                buffer.WriteInt32(0); // Equip Date
                buffer.WriteByte(0); //remaining period 1일
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
        }

        public EzPacket CreateAcceptGift(AcceptGiftResponseType responseType)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32((int) responseType);
            buffer.WriteInt32(0);
            buffer.WriteInt32(0);
            return new EzPacket(50, buffer);
        }
    }
}