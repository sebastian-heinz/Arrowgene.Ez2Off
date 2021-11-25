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

namespace Arrowgene.Ez2Off.Common.Models
{
    /// <summary>
    /// Inventory of a player.
    /// </summary>
    [Serializable]
    public class Inventory
    {
        public const int MaxItems = 15;
        public const int MaxGifts = 15;
        public const int MaxEquipment = 10;
        public const int InvalidSlot = -1;

        private readonly InventoryItem[] _items;
        private readonly InventoryItem[] _equipments;
        private readonly GiftItem[] _gifts;

        public bool ItemsChanged { get; set; }

        public Inventory()
        {
            ItemsChanged = false;
            _items = new InventoryItem[MaxItems];
            _equipments = new InventoryItem[MaxEquipment];
            _gifts = new GiftItem[MaxGifts];
        }

        public void Load(List<InventoryItem> inventoryItems)
        {
            foreach (InventoryItem item in inventoryItems)
            {
                if (item.Slot < MaxItems && item.Slot >= 0)
                {
                    _items[item.Slot] = item;
                    item.Equipped = InvalidSlot;
                }
                else if (item.Equipped < MaxEquipment && item.Equipped >= 0)
                {
                    _equipments[item.Equipped] = item;
                    item.Slot = InvalidSlot;
                }
            }
        }

        public void LoadGiftItems(List<GiftItem> giftItems)
        {
            int count = giftItems.Count;
            for (int i = 0; i < MaxGifts && i < count; i++)
            {
                _gifts[i] = giftItems[i];
            }
        }

        public GiftItem[] GetGiftItems()
        {
            GiftItem[] gifts = new GiftItem[MaxGifts];
            Array.Copy(_gifts, 0, gifts, 0, MaxGifts);
            return gifts;
        }

        public GiftItem GetGiftItem(int index)
        {
            if (index >= MaxGifts || index < 0)
            {
                return null;
            }

            return _gifts[index];
        }

        public GiftItem GetGiftItemById(int giftId)
        {
            for (int i = 0; i < MaxGifts; i++)
            {
                GiftItem gift = _gifts[i];
                if (gift != null && gift.Id == giftId)
                {
                    return gift;
                }
            }

            return null;
        }

        public bool AddGiftItem(GiftItem gift)
        {
            for (int i = 0; i < MaxGifts; i++)
            {
                if (_gifts[i] == null)
                {
                    _gifts[i] = gift;
                    return true;
                }
            }

            return false;
        }

        public bool RemoveGiftItem(GiftItem item)
        {
            for (int i = 0; i < MaxGifts; i++)
            {
                if (_gifts[i] == item)
                {
                    _gifts[i] = null;
                    return true;
                }
            }

            return false;
        }

        public bool HasGifts()
        {
            for (int i = 0; i < MaxGifts; i++)
            {
                GiftItem gift = _gifts[i];
                if (gift != null && !gift.Read)
                {
                    return true;
                }
            }

            return false;
        }

        public InventoryItem[] GetEquipments()
        {
            InventoryItem[] equipments = new InventoryItem[MaxItems];
            Array.Copy(_equipments, equipments, MaxItems);
            return equipments;
        }

        public int GetAvatarId()
        {
            InventoryItem inventoryItem = GetEquiped(ItemType.Avatar);
            if (inventoryItem == null)
            {
                return 0;
            }

            return inventoryItem.Item.Id;
        }

        public int GetSkinId()
        {
            InventoryItem inventoryItem = GetEquiped(ItemType.Skin);
            if (inventoryItem == null)
            {
                return 0;
            }

            return inventoryItem.Item.Id;
        }

        public int GetNoteId()
        {
            InventoryItem inventoryItem = GetEquiped(ItemType.Note);
            if (inventoryItem == null)
            {
                return 0;
            }

            return inventoryItem.Item.Id;
        }

        public int GetPremiumKeyId()
        {
            InventoryItem inventoryItem = GetEquiped(ItemType.Premium3);
            if (inventoryItem == null)
            {
                return 0;
            }

            return inventoryItem.Item.Id;
        }

        public int GetFreeTicketId()
        {
            InventoryItem inventoryItem = GetEquiped(ItemType.Premium4);
            if (inventoryItem == null)
            {
                return 0;
            }

            return inventoryItem.Item.Id;
        }

        public InventoryItem GetEquiped(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Avatar: return GetEquip(0);
                case ItemType.Skin: return GetEquip(1);
                case ItemType.Note: return GetEquip(2);
                case ItemType.Premium1: return GetEquip(3);
                case ItemType.Premium2: return GetEquip(4);
                case ItemType.Premium3: return GetEquip(5);
                case ItemType.Premium4: return GetEquip(6);
                default: return null;
            }
        }

        /**
         * Returns the amount of bonus exp from equipments
         */
        public int GetExpBonusPercentage()
        {
            int percentage = 0;
            for (int i = 0; i <= 6; i++)
            {
                InventoryItem equip = GetEquip(i);
                if (equip != null)
                {
                    percentage += equip.Item.ExpPlus;
                }
            }

            return percentage;
        }

        /**
         * Returns the amount of bonus hp from equipments
         */
        public int GetHpBonusPercentage()
        {
            int percentage = 0;
            for (int i = 0; i <= 6; i++)
            {
                InventoryItem equip = GetEquip(i);
                if (equip != null)
                {
                    percentage += equip.Item.HpPlus;
                }
            }

            return percentage;
        }

        /**
         * Returns the amount of bonus coins from equipments
         */
        public int GetCoinBonusPercentage()
        {
            int percentage = 0;
            for (int i = 0; i <= 6; i++)
            {
                InventoryItem equip = GetEquip(i);
                if (equip != null)
                {
                    percentage += equip.Item.CoinPlus;
                }
            }

            return percentage;
        }

        /// <summary>
        /// Adds an Item to the inventory.
        /// Assigns the slot to the item.
        /// Returns false if the inventory is full.
        /// </summary>
        public bool AddItem(InventoryItem item)
        {
            for (int i = 0; i < MaxItems; i++)
            {
                if (_items[i] == null)
                {
                    _items[i] = item;
                    item.Slot = (byte) i;
                    item.Equipped = InvalidSlot;
                    return true;
                }
            }

            return false;
        }

        public InventoryItem GetItem(int index)
        {
            if (index >= MaxItems || index < 0)
            {
                return null;
            }

            return _items[index];
        }

        public InventoryItem GetItemById(int itemId)
        {
            for (int i = 0; i < MaxItems; i++)
            {
                InventoryItem item = _items[i];
                if (item != null && item.Id == itemId)
                {
                    return item;
                }
            }

            for (int i = 0; i < MaxEquipment; i++)
            {
                InventoryItem item = _equipments[i];
                if (item != null && item.Id == itemId)
                {
                    return item;
                }
            }

            return null;
        }


        public InventoryItem GetEquip(int index)
        {
            if (index >= MaxEquipment || index < 0)
            {
                return null;
            }

            return _equipments[index];
        }

        public bool RemoveItem(InventoryItem item)
        {
            bool removed = false;
            if (item.Slot < MaxItems && item.Slot >= 0)
            {
                _items[item.Slot] = null;
                item.Slot = InvalidSlot;
                removed = true;
            }

            if (item.Equipped < MaxEquipment && item.Equipped >= 0)
            {
                _equipments[item.Equipped] = null;
                item.Equipped = InvalidSlot;
                removed = true;
            }

            return removed;
        }

        public int ItemCount()
        {
            int count = 0;
            for (int i = 0; i < MaxItems; i++)
            {
                InventoryItem item = _items[i];
                if (item != null)
                {
                    count++;
                }
            }

            return count;
        }

        public bool Move(int slotSource, int slotDestination)
        {
            if (slotDestination >= MaxItems)
            {
                return false;
            }

            InventoryItem source = _items[slotSource];
            if (_items[slotDestination] != null)
            {
                InventoryItem destination = _items[slotDestination];
                _items[slotSource] = destination;
                _items[slotDestination] = source;
                source.Slot = (byte) slotDestination;
                destination.Slot = (byte) slotSource;
            }
            else
            {
                _items[slotSource] = null;
                _items[slotDestination] = source;
                source.Slot = (byte) slotDestination;
            }

            return true;
        }

        public bool Equip(int itemSlot, int equipmentSlot)
        {
            if (itemSlot >= MaxItems || equipmentSlot >= MaxEquipment)
            {
                return false;
            }

            InventoryItem item = _items[itemSlot];
            InventoryItem equipped = _equipments[equipmentSlot];

            if (item == null && equipped != null)
            {
                // UnEquip
                _items[itemSlot] = equipped;
                _equipments[equipmentSlot] = null;
                equipped.Slot = itemSlot;
                equipped.Equipped = InvalidSlot;
            }
            else if (item != null && equipped == null)
            {
                // Equip
                _equipments[equipmentSlot] = item;
                _items[itemSlot] = null;
                item.Equipped = equipmentSlot;
                item.Slot = InvalidSlot;
            }
            else if (item != null && equipped != null)
            {
                //Swap
                _items[itemSlot] = equipped;
                equipped.Slot = itemSlot;
                equipped.Equipped = InvalidSlot;
                _equipments[equipmentSlot] = item;
                item.Equipped = equipmentSlot;
                item.Slot = InvalidSlot;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}