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
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Id;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class InventoryAcceptGift : Handler<EzServer>
    {
        public InventoryAcceptGift(EzServer server) : base(server)
        {
        }

        public override int Id => (int) WorldRequestId.InventoryAcceptGift;

        public override void Handle(EzClient client, EzPacket packet)
        {
            byte index = packet.Data.ReadByte();

            GiftItem gift = client.Inventory.GetGiftItem(index);
            
            if (gift == null)
            {
                Logger.Error(client, $"Index: {index} not found");
                SendResponse(client, AcceptGiftResponseType.InvalidItemSlot, index);
                return;
            }

            Item item = Database.SelectItem(gift.ItemId);
            if (item == null)
            {
                Logger.Error(client, $"ItemId: {gift.ItemId} not found");
                SendResponse(client, AcceptGiftResponseType.InvalidItemSlot, index);
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
                SendResponse(client, AcceptGiftResponseType.InvalidItemSlot, index);
                return;
            }

            if (!client.Inventory.AddItem(inventoryItem))
            {
                client.Inventory.AddGiftItem(gift);
                Logger.Error(client, $"Couldn't add Item: {item.Name} to inventory");
                SendResponse(client, AcceptGiftResponseType.CanNotAddItemToInventory, index);
                return;
            }

            if (!Database.InsertInventoryItem(inventoryItem))
            {
                Logger.Error(client, $"Couldn't save Item: {item.Name} to database");
                SendResponse(client, AcceptGiftResponseType.DatabaseError, index);
                return;
            }

            if (!Database.DeleteGiftItem(gift.Id))
            {
                Logger.Error(client, $"Couldn't delete gift: {gift.Id} from database");
                SendResponse(client, AcceptGiftResponseType.DatabaseError, index);
                return;
            }

            Logger.Debug(client, $"Accepted Gift ItemId: {item.Id} Name: {item.Name}");
            SendResponse(client, AcceptGiftResponseType.Success, index);
        }


        private void SendResponse(EzClient client, AcceptGiftResponseType responseType, int index)
        {
            Router.Send(client, PacketBuilder.InventoryPacket.CreateAcceptGift(responseType));

            IBuffer showInventoryPacket = PacketBuilder.InventoryPacket.ShowInventoryPacket(client.Inventory);
            Router.Send(client, 36, showInventoryPacket);
        }
    }
}