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
    public class InventoryPacket
    {
        public static IBuffer ShowInventoryPacket(Inventory inventory)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            WriteEquipSlots(inventory, buffer);
            WritePremiumSlot(inventory, buffer);
            WriteEffectSlot(inventory, buffer);
            for (int i = 0; i < Inventory.MaxItems; i++)
            {
                WriteInventorySlot(i, inventory, buffer);
            }

            return buffer;
        }

        public static void WriteEquipSlots(Inventory inventory, IBuffer buffer)
        {
            WriteEquipSlot(0, inventory, buffer);
            WriteEquipSlot(1, inventory, buffer);
            WriteEquipSlot(2, inventory, buffer);
        }

        public static void WriteEquipSlot(int slot, Inventory inventory, IBuffer buffer)
        {
            InventoryItem item = inventory.GetEquip(slot);
            if (item == null)
            {
                buffer.WriteInt16(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);

                buffer.WriteByte((byte)slot); //ItemSlot
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);

                buffer.WriteByte(0); //remaining period
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteInt16((short)item.Item.Id);
                buffer.WriteByte(0);
                buffer.WriteByte(0);

                buffer.WriteByte(0); //ItemSlot
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);

                buffer.WriteByte(0x01); //remaining period
                buffer.WriteByte(0);
            }
        }

        public static void WritePremiumSlot(Inventory inventory, IBuffer buffer)
        {
            InventoryItem premium1 = inventory.GetEquip(3);
            InventoryItem premium2 = inventory.GetEquip(4);
            InventoryItem premium3 = inventory.GetEquip(5);

            if (premium1 != null)
            {
                buffer.WriteInt16((short)premium1.Item.Id);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0); //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }

            if (premium2 != null)
            {
                buffer.WriteInt16((short)premium2.Item.Id);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0); //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }

            if (premium3 != null)
            {
                buffer.WriteInt16((short)premium3.Item.Id);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0); //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }


            buffer.WriteByte(3); //ItemSlot
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(4); //ItemSlot
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(5); //ItemSlot
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(0x04); //PREMIUM slot1 remaining period
            buffer.WriteByte(0);
            buffer.WriteByte(0x05); //PREMIUM slot2 remaining period
            buffer.WriteByte(0);
            buffer.WriteByte(0x06); //PREMIUM slot3 remaining period
            buffer.WriteByte(0);
        }

        public static void WriteEffectSlot(Inventory inventory, IBuffer buffer)
        {
            InventoryItem effect1 = inventory.GetEquip(6);
            InventoryItem effect2 = inventory.GetEquip(7);
            InventoryItem effect3 = inventory.GetEquip(8);
            InventoryItem effect4 = inventory.GetEquip(9);

            if (effect1 != null)
            {
                buffer.WriteInt16((short)effect1.Item.Id); //Speed //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0); //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }

            if (effect2 != null)
            {
                buffer.WriteInt16((short)effect2.Item.Id); //FADE //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0); //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }

            if (effect3 != null)
            {
                buffer.WriteInt16((short)effect3.Item.Id); //NOTE //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0); //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }


            if (effect4 != null)
            {
                buffer.WriteInt16((short)effect4.Item.Id); //WIDE //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(0); //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }


            buffer.WriteByte(6); //ItemSlot
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(7); //ItemSlot
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(8); //ItemSlot
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(9); //ItemSlot
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(0x07); //effect slot1 remaining period
            buffer.WriteByte(0);
            buffer.WriteByte(0x08); //effect slot2 remaining period
            buffer.WriteByte(0);
            buffer.WriteByte(0x09); //effect slot3 remaining period
            buffer.WriteByte(0);
            buffer.WriteByte(0x0A); //effect slot4 remaining period
            buffer.WriteByte(0);
        }

        public static void WriteInventorySlot(int slot, Inventory inventory, IBuffer buffer)
        {
            InventoryItem item = inventory.GetItem(slot);
            if (item != null)
            {
                buffer.WriteInt32(item.Item.Id); //ItemCode


                buffer.WriteByte((byte)slot); //ItemSlot
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);

                buffer.WriteByte(0); //remaining period
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else
            {
                // Empty Slot
                buffer.WriteByte(0); //ItemCode
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);

                buffer.WriteByte((byte)slot); //ItemSlot
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);

                buffer.WriteByte(0); //remaining period
                buffer.WriteByte(0);
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
        }
    }
}