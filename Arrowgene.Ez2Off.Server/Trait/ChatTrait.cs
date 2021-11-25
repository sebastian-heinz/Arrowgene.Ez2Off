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

namespace Arrowgene.Ez2Off.Server.Trait
{
    public abstract class ChatTrait : EzTrait
    {
        public ChatTrait(EzServer server) : base(server)
        {
        }

        public virtual void Send(List<ChatMessage> messages)
        {
            foreach (ChatMessage message in messages)
            {
                Send(message);
            }
        }

        public virtual void Send(ChatMessage message)
        {
            if (!IsDeliverable(message))
            {
                return;
            }

            switch (message.Type)
            {
                case ChatType.Lobby:
                    SendChannel(message);
                    break;
                case ChatType.Room:
                    SendRoom(message);
                    break;
                case ChatType.Whisper:
                    SendWhisper(message);
                    break;
                case ChatType.Direct:
                    SendDirect(message);
                    break;
                case ChatType.Gm:
                    SendGm(message);
                    break;
                default:
                    Logger.Debug($"Unknown ChatType: {message.Type}");
                    break;
            }
        }

        public virtual bool IsDeliverable(ChatMessage message)
        {
            if (message.Recipients.Count <= 0)
            {
                Logger.Debug("Message has no recipients");
                return false;
            }

            return message.Deliver;
        }

        public abstract void SendChannel(ChatMessage message);
        public abstract void SendRoom(ChatMessage message);
        public abstract void SendWhisper(ChatMessage message);
        public abstract void SendDirect(ChatMessage message);
        public abstract void SendGm(ChatMessage message);
    }
}