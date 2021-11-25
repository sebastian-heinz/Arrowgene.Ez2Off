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

namespace Arrowgene.Ez2Off.Common.Models
{
    /// <summary>
    /// Item that is owned by a player.
    /// </summary>
    [Serializable]
    public class InventoryItem
    {
        public InventoryItem()
        {
            Id = -1;
            Slot = -1;
            Equipped = -1;
            CharacterId = -1;
            EquipDate = null;
        }

        public int Id { get; set; }
        public int CharacterId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? EquipDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public Item Item { get; set; }
        public int Equipped { get; set; }
        public int Slot { get; set; }

        public int GetEquipDateUnixTime()
        {
            if (EquipDate.HasValue)
            {
                return (int) Utils.GetUnixTime(EquipDate.Value);
            }

            return 0;
        }

        public bool IsEquipped()
        {
            return Equipped != Inventory.InvalidSlot;
        }

        /// <summary>
        /// Sets the item to status "Used"
        /// </summary>
        public void MarkUsed()
        {
            if (!ExpireDate.HasValue)
            {
                DateTime now = DateTime.Now;
                EquipDate = now;
                ExpireDate = now.AddDays(Item.Duration);
            }
        }

        public bool IsUsed()
        {
            return ExpireDate.HasValue;
        }

        public int UsedDayCount()
        {
            int usedDays = 0;
            if (EquipDate.HasValue)
            {
                TimeSpan duration = DateTime.Now - EquipDate.Value;
                usedDays = (int) duration.TotalDays;
            }

            return Item.Duration - usedDays;
        }
    }
}