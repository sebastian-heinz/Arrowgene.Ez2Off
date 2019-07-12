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
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class InventoryDeleteItem : Handler<WorldServer>
    {
        public InventoryDeleteItem(WorldServer server) : base(server)
        {
        }

        public override int Id => 0x17;

        public override void Handle(EzClient client, EzPacket packet)
        {
            byte unknown = packet.Data.ReadByte();
            short itemId = packet.Data.ReadInt16(Endianness.Big);
            byte itemSlot = packet.Data.ReadByte();

            InventoryItem inventoryItem = client.Inventory.GetItem(itemSlot);
            if (inventoryItem == null)
            {
                _logger.Error("Item could not be found");
                return;
            }

            if (inventoryItem.Item.Id != itemId)
            {
                _logger.Error("InventoryItem Id {0} does not match clients {1}", inventoryItem.Item.Id, itemId);
                return;
            }

            if (!client.Inventory.RemoveItem(inventoryItem))
            {
                _logger.Error("Could not remove item {0} from bag", inventoryItem.Item.Name);
                return;
            }

            if (!Database.DeleteInventoryItem(inventoryItem.Id))
            {
                _logger.Error("Could not deleye item {0} from database", inventoryItem.Item.Name);
                return;
            }

            _logger.Debug("Deleted Item {0} from slot {1}", inventoryItem.Item.Name, inventoryItem.Slot);
            IBuffer showInventoryPacket = InventoryPacket.ShowInventoryPacket(client.Inventory);
            Send(client, 0x1E, showInventoryPacket);
        }
    }
}