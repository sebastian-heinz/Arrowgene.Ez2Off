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

namespace Arrowgene.Ez2Off.Common.Models
{
    /// <summary>
    /// Inventory of a player.
    /// </summary>
    [Serializable]
    public class Inventory
    {
        public const int MaxItems = 15;
        public const int MaxEquipment = 10;
        public const int InvalidSlot = -1;

        private readonly InventoryItem[] _items;
        private readonly InventoryItem[] _equipments;

        public Inventory()
        {
            _items = new InventoryItem[MaxItems];
            _equipments = new InventoryItem[MaxEquipment];
        }

        public Inventory(List<InventoryItem> inventoryItems) : this()
        {
            LoadItems(inventoryItems);
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

        public InventoryItem GetEquiped(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Avatar: return GetEquip(0);
                case ItemType.Skin: return GetEquip(1);
                case ItemType.Note: return GetEquip(2);
                default: return null;
            }
        }

        /// <summary>
        /// Adds an Item to the inventory.
        /// Assings the slot to the item.
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

        private void LoadItems(List<InventoryItem> inventoryItems)
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
    }
}