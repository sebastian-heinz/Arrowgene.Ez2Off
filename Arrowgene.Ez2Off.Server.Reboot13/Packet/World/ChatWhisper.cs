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
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.World
{
    /// <summary>
    /// Private chat with pink font.
    /// </summary>
    public class PrivateChat : Handler<EzServer>
    {
        public PrivateChat(EzServer server) : base(server)
        {
        }

        public override int Id => 12;

        public override void Handle(EzClient client, EzPacket packet)
        {
            string receiverName = packet.Data.ReadFixedString(17, Utils.KoreanEncoding);
            byte messageLength = packet.Data.ReadByte();
            string message = packet.Data.ReadString(messageLength, Utils.KoreanEncoding);

            EzClient receiver = Server.Clients.GetClient(receiverName);

            if (receiver == null)
            {
                Logger.Error(client, $"Receiver Character: {receiverName} doesn't exist or is offline");
                return;
            }

            Server.Chat.Handle(client, message, ChatType.Whisper, client, receiver);
        }
    }
}