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
using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class ShopPurchaseItem : Handler<EzServer>
    {
        public ShopPurchaseItem(EzServer server) : base(server)
        {
        }

        public override int Id => 20;

        public override void Handle(EzClient client, EzPacket packet)
        {
            bool shoppingCart = packet.Data.ReadByte() > 0;
            List<Item> items = ReadShoppingCart(packet);
            Logger.Debug(client, $"ShoppingCart: {shoppingCart}");

            int totalCoins = 0;
            int totalCash = 0;
            foreach (Item item in items)
            {
                if (item.Currency == ItemCurrencyType.Cash)
                {
                    totalCash += item.Price;
                }
                else
                {
                    totalCoins += item.Price;
                }
            }

            if (client.Character.Coin < totalCoins)
            {
                Logger.Debug(client, $"Coins: {client.Character.Coin} to less to buy for Coins: {totalCoins}");
                return;
            }

            if (client.Character.Cash < totalCash)
            {
                Logger.Debug(client, $"Cash: {client.Character.Cash} to less to buy for Cash: {totalCash}");
                return;
            }

            if (!HasSpace(items, client.Inventory))
            {
                Logger.Debug(client, "Not enough space, cancel purchase");
                Router.Send(client, 29,
                    PacketBuilder.ShopPacket.CreatePurchasePacket(null, client.Character,
                        ShopPurchaseItemMessageType.SlotFull));
                return;
            }

            foreach (Item item in items)
            {
                InventoryItem inventoryItem = new InventoryItem();
                inventoryItem.CharacterId = client.Account.Id;
                inventoryItem.PurchaseDate = DateTime.Now;
                inventoryItem.Equipped = -1;
                inventoryItem.Item = item;
                inventoryItem.Slot = -1;
                inventoryItem.Id = -1;
                if (!client.Inventory.AddItem(inventoryItem))
                {
                    Logger.Error(client, $"Couldn't add Item: {item.Name} to inventory");
                    continue;
                }

                if (!Database.InsertInventoryItem(inventoryItem))
                {
                    Logger.Error(client, $"Couldn't save Item: {item.Name} to database");
                    continue;
                }

                Logger.Debug(client, $"Purchased ItemId: {item.Id} ItemName: {item.Name}");
                if (item.Currency == ItemCurrencyType.Cash)
                {
                    client.Character.Cash -= item.Price;
                }
                else
                {
                    client.Character.Coin -= item.Price;
                }

                Router.Send(client, 29,
                    PacketBuilder.ShopPacket.CreatePurchasePacket(item, client.Character,
                        ShopPurchaseItemMessageType.Success));
            }
        }

        private List<Item> ReadShoppingCart(EzPacket packet)
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i < 5; i++)
            {
                Item item = ReadItem(packet);
                if (item != null)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        private Item ReadItem(EzPacket packet)
        {
            short itemId = packet.Data.ReadInt16(Endianness.Big);
            if (itemId > 0)
            {
                return Database.SelectItem(itemId);
            }

            return null;
        }

        /// <summary>
        /// Check if the inventory has enough space to hold the item.
        /// </summary>
        private bool HasSpace(List<Item> items, Inventory inventory)
        {
            return inventory.ItemCount() + items.Count <= Inventory.MaxItems;
        }
    }
}