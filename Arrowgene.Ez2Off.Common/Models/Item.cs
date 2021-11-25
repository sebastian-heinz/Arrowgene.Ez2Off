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
    [Serializable]
    public class Item
    {
        public const double SellFactor = 1.42857;

        /// <summary>
        /// 1 Cash = 58.333 Coins
        /// </summary>
        public const double ConvertFactor = 58.333;

        public Item()
        {
            Id = -1;
        }

        public ItemCurrencyType Currency { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }
        public int Level { get; set; }

        public bool Enabled { get; set; }

        /// <summary>
        /// 경험치
        /// </summary>
        public int ExpPlus { get; set; }

        public int CoinPlus { get; set; }
        public int HpPlus { get; set; }

        /// <summary>
        /// 회복력
        /// </summary>
        public int ResiliencePlus { get; set; }

        /// <summary>
        /// 방어력
        /// </summary>
        public int DefensePlus { get; set; }

        public string Effect { get; set; }
        public int Id { get; set; }
        public ItemType Type { get; set; }
    }
}