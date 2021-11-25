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

using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Chat.Messages;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder;
using Arrowgene.Ez2Off.Server.Trait;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Trait
{
    public class R13ChatTrait : ChatTrait
    {
        public R13ChatTrait(EzServer server) : base(server)
        {
        }

        public override void SendChannel(ChatMessage message)
        {
            IBuffer packet = ChatPacket.CreateChannel(message.SenderName, message.Message);
            Router.Send(message.Recipients, 18, packet);
        }

        public override void SendRoom(ChatMessage message)
        {
            byte slot = 0;
            if (message is PlayerChatMessage playerMessage)
            {
                slot = (byte) playerMessage.Sender.Player.Slot;
            }

            IBuffer packet = ChatPacket.CreateRoom(slot, message.SenderName, message.Message);
            Router.Send(message.Recipients, 18, packet);
        }

        public override void SendWhisper(ChatMessage message)
        {
            IBuffer packet = ChatPacket.CreateWhisper(message.SenderName, message.Message);
            Router.Send(message.Recipients, 20, packet);
        }

        public override void SendDirect(ChatMessage message)
        {
            if (message.Recipients[0] == null)
            {
                return;
            }

            EzClient receiver = message.Recipients[0];

            IBuffer packet = ChatPacket.CreateDirect(message.SenderName, receiver.Character.Name, message.Message,
                ChatDirectResponseType.Success);
            Router.Send(message.Recipients, 20, packet);
        }

        public override void SendGm(ChatMessage message)
        {
            IBuffer packet = ChatPacket.CreateGm(message.Message);
            Router.Send(message.Recipients, 21, packet);
        }
    }
}