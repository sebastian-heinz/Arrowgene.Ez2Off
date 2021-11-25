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

namespace Arrowgene.Ez2Off.Server.Chat.Messages
{
    public class ChatMessage
    {
        public const int ChannelLength = 34;
        public const int GameLength = 30;

        /// <summary>
        /// Splits a long message into multiple small ones.
        /// </summary>
        public static List<ChatMessage> MultiPart(string sender, string message, ChatType type,
            List<EzClient> recipients, int maxLen = 0)
        {
            if (maxLen <= 0)
            {
                switch (type)
                {
                    case ChatType.Lobby:
                        maxLen = ChannelLength;
                        break;
                    case ChatType.Room:
                        maxLen = GameLength;
                        break;
                    case ChatType.Whisper:
                        maxLen = ChannelLength;
                        break;
                    case ChatType.Direct:
                        maxLen = ChannelLength;
                        break;
                    default:
                        maxLen = GameLength;
                        break;
                }
            }

            List<ChatMessage> messages = new List<ChatMessage>();
            while (message.Length > maxLen)
            {
                string cutMessage = message.Substring(0, maxLen);
                message = message.Substring(maxLen, message.Length - maxLen);
                messages.Add(new ChatMessage(sender, cutMessage, type, recipients));
            }

            messages.Add(new ChatMessage(sender, message, type, recipients));
            return messages;
        }

        /// <summary>
        /// Creates a message with custom sender name and text.
        /// The response type and recipients are based on the provided message.
        /// </summary>
        public static List<ChatMessage> SystemResponse(string sender, string text, ChatMessage message)
        {
            return MultiPart(sender, text, message.Type, message.Recipients);
        }

        public DateTime Date { get; }
        public ChatType Type { get; }
        public string SenderName { get; }
        public List<EzClient> Recipients { get; }
        public string OriginalMessage { get; }
        public bool Deliver { get; set; }
        public string Message { get; set; }

        public ChatMessage(string senderName, string message, ChatType chatType, params EzClient[] recipients)
            : this(senderName, message, chatType, new List<EzClient>(recipients))
        {
        }

        public ChatMessage(string senderName, string message, ChatType chatType, List<EzClient> recipients)
        {
            Recipients = new List<EzClient>();
            Date = new DateTime();
            Deliver = true;
            Message = message;
            OriginalMessage = message;
            SenderName = senderName;
            Type = chatType;
            if (recipients != null)
            {
                Recipients.AddRange(recipients);
            }
        }
    }
}