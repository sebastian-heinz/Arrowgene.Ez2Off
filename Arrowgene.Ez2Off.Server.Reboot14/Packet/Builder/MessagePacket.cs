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
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Packet.Builder;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder
{
    public class MessagePacket : IMessagePacket
    {
        public IBuffer CreateMessageBox(List<Message> messages)
        {
            IBuffer buffer = EzServer.Buffer.Provide();

            buffer.WriteInt32(messages.Count);
            foreach (Message message in messages)
            {
                WriteMessage(buffer, message);
            }

            return buffer;
        }

        public IBuffer CreateNewMessageNotification()
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            return buffer;
        }

        public IBuffer CreateMessageResponse(MessageResponseType responseType)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte((byte) responseType);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            return buffer;
        }

        public void WriteMessage(IBuffer buffer, Message message)
        {
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteInt32(message.Id);
            buffer.WriteInt32(message.Read ? 0 : 1);

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteFixedString(message.Sender, 18, Utils.KoreanEncoding);
            buffer.WriteFixedString(message.Content, 128, Utils.KoreanEncoding);
            buffer.WriteInt32((int) Utils.GetUnixTime(message.SendAt));
        }

        public IBuffer AddFriend(string characterName, FriendAddMessageType messageType)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32((byte) messageType);
            buffer.WriteFixedString(characterName, 18, Utils.KoreanEncoding);
            buffer.WriteByte(0);
            return buffer;
        }

        public IBuffer DeleteFriend(string characterName, FriendDeleteMessageType messageType)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32((byte) messageType);
            buffer.WriteFixedString(characterName, 18, Utils.KoreanEncoding);
            buffer.WriteByte(0);
            return buffer;
        }
    }
}