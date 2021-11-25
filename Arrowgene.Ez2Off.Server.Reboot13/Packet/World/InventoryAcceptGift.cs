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


using System;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.World
{
    public class InventoryAcceptGift : Handler<EzServer>
    {
        public InventoryAcceptGift(EzServer server) : base(server)
        {
        }

        public override int Id => 37;

        public override void Handle(EzClient client, EzPacket packet)
        {
            byte index = packet.Data.ReadByte();

            GiftItem gift = client.Inventory.GetGiftItem(index);

            if (gift == null)
            {
                Logger.Error(client, $"Index: {index} not found");
                return;
            }

            Item item = Database.SelectItem(gift.ItemId);
            if (item == null)
            {
                Logger.Error(client, $"ItemId: {gift.ItemId} not found");
                return;
            }

            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.CharacterId = client.Account.Id;
            inventoryItem.PurchaseDate = DateTime.Now;
            inventoryItem.Equipped = -1;
            inventoryItem.Item = item;
            inventoryItem.Slot = -1;
            inventoryItem.Id = -1;

            if (!client.Inventory.RemoveGiftItem(gift))
            {
                Logger.Error(client, $"Couldn't remove Item: {item.Name}");
                return;
            }

            if (!client.Inventory.AddItem(inventoryItem))
            {
                Logger.Error(client, $"Couldn't add Item: {item.Name} to inventory");
                return;
            }

            if (!Database.InsertInventoryItem(inventoryItem))
            {
                Logger.Error(client, $"Couldn't save Item: {item.Name} to database");
                return;
            }

            Logger.Debug(client, $"Accepted Gift ItemId: {item.Id} Name: {item.Name}");
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32(1); // Message Type; 
            /*  1 = Accept 
                100 = Invalid Item number or Empty Item slot
                101 = Couldn't add item to inventory
                102 = Expired convert item
                999 =  DB Error messageBox*/
            buffer.WriteInt32(0); // slot Index;
            buffer.WriteInt32(0); // Unknown
            Router.Send(client, 51, buffer);
            IBuffer showInventoryPacket = InventoryPacket.ShowInventoryPacket(client.Inventory);
            Router.Send(client, 36, showInventoryPacket); //  가방 열림,목록
        }
    }
}