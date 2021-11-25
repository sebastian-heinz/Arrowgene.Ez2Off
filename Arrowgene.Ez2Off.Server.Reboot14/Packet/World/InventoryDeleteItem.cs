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
    public class InventoryDeleteItem : Handler<EzServer>
    {
        public InventoryDeleteItem(EzServer server) : base(server)
        {
        }

        public override int Id => 23;

        public override void Handle(EzClient client, EzPacket packet)
        {
            byte unknown = packet.Data.ReadByte();
            short itemId = packet.Data.ReadInt16(Endianness.Big);
            byte itemSlot = packet.Data.ReadByte();

            InventoryItem inventoryItem = client.Inventory.GetItem(itemSlot);
            if (inventoryItem == null)
            {
                Logger.Error(client, $"InventoryItem Slot: {itemSlot} couldn't be found");
                return;
            }

            if (inventoryItem.Item.Id != itemId)
            {
                Logger.Error(client, $"InventoryItem Id: {inventoryItem.Item.Id} does not match clients Id: {itemId}");
                return;
            }

            if (!client.Inventory.RemoveItem(inventoryItem))
            {
                Logger.Error(client, $"Couldn't remove InventoryItem: {inventoryItem.Item.Name} from bag");
                return;
            }

            if (!Database.DeleteInventoryItem(inventoryItem.Id))
            {
                Logger.Error(client, $"Couldn't save InventoryItem: {inventoryItem.Item.Name} from database");
                return;
            }

            int refund;
            if (inventoryItem.IsUsed())
            {
                refund = 0;
            }
            else if (inventoryItem.Item.Currency == ItemCurrencyType.Cash)
            {
                refund = (int) (inventoryItem.Item.Price * Item.ConvertFactor);
            }
            else
            {
                refund = (int) (inventoryItem.Item.Price / Item.SellFactor);
            }

            client.Character.Coin += refund;

            Logger.Debug(client,
                $"Deleted InventoryItem: {inventoryItem.Item.Name} from Slot: {inventoryItem.Slot}. Refund {refund} Coin.");

            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32(0); // 0=Success 3=Inventory full
            buffer.WriteByte(0);
            buffer.WriteByte(itemSlot); // Index
            buffer.WriteInt16(0); // ItemId
            buffer.WriteInt32(client.Character.Coin);
            Router.Send(client, 35, buffer);
        }
    }
}