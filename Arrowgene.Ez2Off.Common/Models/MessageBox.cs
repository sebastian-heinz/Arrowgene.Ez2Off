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
    public class MessageBox
    {
        public const int MaxMessages = 15;
        public const int MaxFriends = 15;

        private readonly object _lock = new object();
        private List<Message> _messages;

        public MessageBox()
        {
            _messages = new List<Message>();
        }

        public void Load(List<Message> messages)
        {
            lock (_lock)
            {
                _messages.Clear();
                foreach (Message message in messages)
                {
                    AddMessage(message);
                }
            }
        }

        public List<Message> GetMessages()
        {
            List<Message> messages;
            lock (_lock)
            {
                messages = new List<Message>(_messages);
            }

            return messages;
        }

        public Message GetMessage(int messageId)
        {
            Message result = null;
            lock (_lock)
            {
                foreach (Message message in _messages)
                {
                    if (message.Id == messageId)
                    {
                        result = message;
                    }
                }
            }

            return result;
        }

        public bool AddMessage(Message message)
        {
            if (message == null)
            {
                return false;
            }

            lock (_lock)
            {
                if (_messages.Count >= MaxMessages)
                {
                    return false;
                }

                _messages.Add(message);
            }

            return false;
        }

        public void RemoveMessage(int messageId)
        {
            Message message = GetMessage(messageId);
            RemoveMessage(message);
        }

        public void RemoveMessage(Message message)
        {
            if (message == null)
            {
                return;
            }

            lock (_lock)
            {
                _messages.Remove(message);
            }
        }

        public bool HasUnread()
        {
            List<Message> messages = GetMessages();
            foreach (Message message in messages)
            {
                if (!message.Read)
                {
                    return true;
                }
            }

            return false;
        }
    }
}