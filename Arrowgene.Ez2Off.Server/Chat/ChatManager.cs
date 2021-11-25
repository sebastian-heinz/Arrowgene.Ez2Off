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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Chat.Messages;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Trait;

namespace Arrowgene.Ez2Off.Server.Chat
{
    public class ChatManager
    {
        private readonly List<IChatMiddleware> _middleware;
        private readonly ChatTrait _chatTrait;

        public ChatManager(ChatTrait chatTrait)
        {
            _chatTrait = chatTrait;
            _middleware = new List<IChatMiddleware>();
        }

        public void AddMiddleware(IChatMiddleware middleware)
        {
            _middleware.Add(middleware);
        }

        public void RemoveMiddleware(IChatMiddleware middleware)
        {
            _middleware.Remove(middleware);
        }

        public void Handle(EzClient sender, string message, ChatType chatType, params EzClient[] recipients)
        {
            ChatMessage chatMessage = new PlayerChatMessage(sender, message, chatType, recipients);
            Handle(chatMessage);
        }

        public void Handle(EzClient sender, string message, ChatType chatType, List<EzClient> recipients)
        {
            ChatMessage chatMessage = new PlayerChatMessage(sender, message, chatType, recipients);
            Handle(chatMessage);
        }

        public void Handle(ChatMessage message)
        {
            List<ChatMessage> messages = new List<ChatMessage>();
            foreach (IChatMiddleware middleware in _middleware)
            {
                middleware.HandleMessage(message, messages);
            }

            _chatTrait.Send(message);
            _chatTrait.Send(messages);
        }

        public void SendGmNotice(string message, List<EzClient> recipients)
        {
            ChatMessage notice = new ChatMessage("", message, ChatType.Gm, recipients);
            Handle(notice);
        }
    }
}