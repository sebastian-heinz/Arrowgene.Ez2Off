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
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class InventoryApplyItem : Handler<EzServer>
    {
        public InventoryApplyItem(EzServer server) : base(server)
        {
        }

        public override int Id => 21;

        public override void Handle(EzClient client, EzPacket packet)
        {
            int itemSlot = packet.Data.ReadInt32();
            int equipmentSlot = packet.Data.ReadInt32();
            packet.Data.ReadInt32();

            InventoryItem item = client.Inventory.GetItem(itemSlot);
            InventoryItem equip = client.Inventory.GetEquip(equipmentSlot);


            if (!client.Inventory.Equip(itemSlot, equipmentSlot))
            {
                Logger.Error(client, $"Couldn't apply ItemSlot {itemSlot} and EquipmentSlot {equipmentSlot}");
                return;
            }

            if (item != null)
            {
                item.MarkUsed();
                if (!Database.UpdateInventoryItem(item))
                {
                    Logger.Error(client, $"Couldn't save Item: {item.Item.Name} to database");
                    return;
                }
            }

            if (equip != null && !Database.UpdateInventoryItem(equip))
            {
                Logger.Error(client, $"Couldn't save Item: {equip.Item.Name} to database");
                return;
            }

            if (client.Room != null)
            {
                Router.Send(client.Room, 31,
                    PacketBuilder.RoomPacket.ItemUpdate((byte) client.Player.Slot, client.Inventory));
            }

            Logger.Debug(client, $"Applied ItemSlot: {itemSlot} and EquipmentSlot: {equipmentSlot}");
            IBuffer showInventoryPacket = PacketBuilder.InventoryPacket.ShowInventoryPacket(client.Inventory);
            Router.Send(client, 30, showInventoryPacket);
        }
    }
}