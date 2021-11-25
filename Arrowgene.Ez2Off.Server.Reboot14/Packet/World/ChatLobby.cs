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

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class ChatLobby : Handler<EzServer>
    {
        public ChatLobby(EzServer server) : base(server)
        {
        }

        public override int Id => 11;

        public override void Handle(EzClient client, EzPacket packet)
        {
            ChatType chatType = (ChatType) packet.Data.ReadByte();
            string sender = packet.Data.ReadFixedString(18, Utils.KoreanEncoding);
            byte messageLength = packet.Data.ReadByte();
            // Last byte is 0x00
            messageLength = (byte) (messageLength - 1);
            string message = packet.Data.ReadString(messageLength, Utils.KoreanEncoding);

            if (sender != client.Character.Name)
            {
                Logger.Error(client,
                    $"Sender Character: {sender} doesn't match client Character: {client.Character.Name}");
            }

            switch (chatType)
            {
                case ChatType.Lobby:
                    Server.Chat.Handle(client, message, chatType, client.Channel.GetLobbyClients());
                    break;
                case ChatType.Room:
                    Server.Chat.Handle(client, message, chatType, client.Room.GetClients());
                    break;
                default:
                    Logger.Error(client, $"Unknown ChatType: {chatType}");
                    break;
            }
        }
    }
}