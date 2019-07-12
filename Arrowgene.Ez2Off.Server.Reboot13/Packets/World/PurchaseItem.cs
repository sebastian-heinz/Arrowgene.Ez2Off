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

using System;
using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class PurchaseItem : Handler<WorldServer>
    {
        public PurchaseItem(WorldServer server) : base(server)
        {
        }

        public override int Id => 0x14;

        public override void Handle(EzClient client, EzPacket packet)
        {
            bool shoppingCart = packet.Data.ReadByte() > 0 ? true : false;
            List<Item> items = ReadShoppingCart(packet);
            _logger.Debug("ShoppingCart: {0}", shoppingCart);


            if (!HasMoney(items, client.Character))
            {
                _logger.Debug("Not enough coins, cancel purchase");
                return;
            }

            if (!HasSpace(items, client.Inventory))
            {
                _logger.Debug("Not enough space, cancel purchase");
                return;
            }

            foreach (Item item in items)
            {
                InventoryItem inventoryItem = new InventoryItem();
                inventoryItem.AccountId = client.Account.Id;
                inventoryItem.PurchaseDate = DateTime.Now;
                inventoryItem.Equipped = -1;
                inventoryItem.Item = item;
                inventoryItem.Slot = -1;
                inventoryItem.Id = -1;
                if (!client.Inventory.AddItem(inventoryItem))
                {
                    _logger.Error("Couldn't add item to inventory {0}", item.Name);
                    continue;
                }

                if (!Database.InsertInventoryItem(inventoryItem))
                {
                    _logger.Error("Couldn't save item to database {0}", item.Name);
                    continue;
                }

                _logger.Debug("Purchased Id: {0} Name: {1}", item.Id, item.Name);
                IBuffer purchasePacket = ShopPacket.CreatePurchasePacket(item);
                Send(client, 0x1D, purchasePacket);
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
        /// Check if the character has the money to purchase the item.
        /// </summary>
        private bool HasMoney(List<Item> items, Character character)
        {
            int totalCoins = 0;
            foreach (Item item in items)
            {
                totalCoins += item.Coins;
            }

            return character.Coin >= totalCoins;
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