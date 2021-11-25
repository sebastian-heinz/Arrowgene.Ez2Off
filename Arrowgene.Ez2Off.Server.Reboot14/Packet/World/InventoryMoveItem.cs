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
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class InventoryMoveItem : Handler<EzServer>
    {
        public InventoryMoveItem(EzServer server) : base(server)
        {
        }

        public override int Id => 22;

        public override void Handle(EzClient client, EzPacket packet)
        {
            byte unknown = packet.Data.ReadByte();
            byte sourceSlot = packet.Data.ReadByte();
            int itemId = packet.Data.ReadInt16(Endianness.Big);
            byte destinationSlot = packet.Data.ReadByte();

            InventoryItem source = client.Inventory.GetItem(sourceSlot);
            InventoryItem destination = client.Inventory.GetItem(destinationSlot);

            if (source == null)
            {
                Logger.Error(client, $"SourceSlot: {sourceSlot} couldn't be found");
                return;
            }

            if (!client.Inventory.Move(sourceSlot, destinationSlot))
            {
                Logger.Error(client,
                    $"Couldn't move Item: {source.Item.Name} from Slot: {sourceSlot} to Slot: {destinationSlot}, destination is occupied");
                return;
            }

            if (!Database.UpdateInventoryItem(source))
            {
                Logger.Error(client, $"Couldn't save source Item: {source.Item.Name} update to database");
                return;
            }

            if (destination != null && !Database.UpdateInventoryItem(destination))
            {
                Logger.Error(client, $"Couldn't save destination Item: {source.Item.Name} update to database");
                return;
            }

            Logger.Debug(client, $"Moved ItemId: {itemId} from Slot: {sourceSlot} to Slot: {destinationSlot}");
            IBuffer showInventoryPacket =  PacketBuilder.InventoryPacket.ShowInventoryPacket(client.Inventory);
            Router.Send(client, 30, showInventoryPacket);
        }
    }
}