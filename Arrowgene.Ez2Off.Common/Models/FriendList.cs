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
    [Serializable]
    public class FriendList
    {
        private List<Friend> _friends;

        public FriendList()
        {
            _friends = new List<Friend>();
        }

        public int Count
        {
            get { return _friends.Count; }
        }

        public void Load(List<Friend> friends)
        {
            _friends.Clear();
            _friends.AddRange(friends);
        }

        public List<Friend> GetAll()
        {
            return new List<Friend>(_friends);
        }

        public void Add(Friend friend)
        {
            _friends.Add(friend);
        }

        public void Remove(Friend friend)
        {
            _friends.Remove(friend);
        }

        public Friend Get(string characterName)
        {
            foreach (Friend friend in _friends)
            {
                if (friend.FriendCharacterName == characterName)
                {
                    return friend;
                }
            }

            return null;
        }

        public Friend Get(int characterId)
        {
            foreach (Friend friend in _friends)
            {
                if (friend.FriendCharacterId == characterId)
                {
                    return friend;
                }
            }

            return null;
        }
    }
}