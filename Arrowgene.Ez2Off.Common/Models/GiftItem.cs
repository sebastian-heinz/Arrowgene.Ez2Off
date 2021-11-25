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
    public class GiftItem
    {
        public static int ExpireDays = 30;

        public GiftItem()
        {
            Id = -1;
            SenderId = -1;
            ReceiverId = -1;
            Read = false;
            ExpireDate = null;
        }

        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public bool Read { get; set; }
        public DateTime SendAt { get; set; }
        public DateTime? ExpireDate { get; set; }

        public int GetExpireDateUnixTime()
        {
            if (ExpireDate.HasValue)
            {
                return (int) Utils.GetUnixTime(ExpireDate.Value);
            }

            return 0;
        }
    }
}