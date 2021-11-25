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

using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Packet.Builder;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder
{
    public class ChatPacket : IChatPacket
    {
        public IBuffer CreateChannel(string sender, string message)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte((byte) ChatType.Lobby);
            buffer.WriteFixedString(sender, 18, Utils.KoreanEncoding);
            WriteMessage(buffer, message);
            return buffer;
        }

        public IBuffer CreateRoom(byte playerSlot, string sender, string message)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(playerSlot);
            buffer.WriteFixedString(sender, 18, Utils.KoreanEncoding);
            WriteMessage(buffer, message);
            return buffer;
        }

        public IBuffer CreateWhisper(string sender, string message)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteFixedString(sender, 18, Utils.KoreanEncoding);
            WriteMessage(buffer, message);
            return buffer;
        }

        public IBuffer CreateDirect(string sender, string receiver, string message,
            ChatDirectResponseType responseType)
        {
            IBuffer response = EzServer.Buffer.Provide();
            response.WriteInt32(0);
            response.WriteInt32(0);
            response.WriteFixedString(sender, 18, Utils.KoreanEncoding);
            response.WriteFixedString(receiver, 18, Utils.KoreanEncoding);
            response.WriteFixedString(message, 255, Utils.KoreanEncoding);
            response.WriteByte(0);
            response.WriteByte((byte) responseType);
            return response;
        }

        public IBuffer CreateGm(string message)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte((byte) 1);
            WriteMessage(buffer, message);
            return buffer;
        }

        private void WriteMessage(IBuffer buffer, string message)
        {
            byte[] msg = Utils.KoreanEncoding.GetBytes(message);
            buffer.WriteByte((byte) (msg.Length + 1));
            buffer.WriteBytes(msg);
            buffer.WriteByte(0);
        }
    }
}