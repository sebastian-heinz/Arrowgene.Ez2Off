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
    public class InventoryApplyItem : Handler<WorldServer>
    {
        public InventoryApplyItem(WorldServer server) : base(server)
        {
        }

        public override int Id => 0x15;

        public override void Handle(EzClient client, EzPacket packet)
        {
            int itemSlot = packet.Data.ReadInt32();
            int equipmentSlot = packet.Data.ReadInt32();
            packet.Data.ReadInt32();

            InventoryItem item = client.Inventory.GetItem(itemSlot);
            InventoryItem equip = client.Inventory.GetEquip(equipmentSlot);

            if (!client.Inventory.Equip(itemSlot, equipmentSlot))
            {
                _logger.Error("couldn't apply itemSlot {0} and equipmentSlot {1}", itemSlot, equipmentSlot);
                return;
            }

            if (item != null && !Database.UpdateInventoryItem(item))
            {
                _logger.Error("Couldn't save item to database {0}", item.Item.Name);
                return;
            }

            if (equip != null && !Database.UpdateInventoryItem(equip))
            {
                _logger.Error("Couldn't save item to database {0}", equip.Item.Name);
                return;
            }

            _logger.Debug("applied itemSlot {0} and equipmentSlot {1}", itemSlot, equipmentSlot);
            IBuffer showInventoryPacket = InventoryPacket.ShowInventoryPacket(client.Inventory);
            Send(client, 0x1E, showInventoryPacket);
        }
    }
}