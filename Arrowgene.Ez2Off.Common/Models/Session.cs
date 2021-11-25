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
    public class Session
    {
        public string Key { get; }
        public DateTime Creation { get; }
        public Account Account { get; }
        public Character Character { get; }
        public Setting Setting { get; }
        public Inventory Inventory { get; }
        public MessageBox MessageBox { get; }
        public FriendList Friends { get; }

        public int ServerId { get; set; }
        public ModeType Mode { get; set; }
        public int ChannelId { get; set; }

        public Session(string sessionKey, Account account)
        {
            Key = sessionKey;
            Creation = DateTime.Now;
            Account = account;
            Character = new Character();
            MessageBox = new MessageBox();
            Inventory = new Inventory();
            Friends = new FriendList();
            Setting = new Setting();
        }
    }
}