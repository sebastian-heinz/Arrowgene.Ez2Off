/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2018 Sebastian Heinz
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

using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class BackButton : Handler<WorldServer>
    {
        public BackButton(WorldServer server) : base(server)
        {
        }

        public override int Id => 8; //뒤로 가기

        public override void Handle(EzClient client, EzPacket packet)
        {
            if (client.Player.Playing)
            {
                client.Player.Playing = false;
                client.Player.Ready = false;
            }
            else
            {
                IBuffer response = EzServer.Buffer.Provide();
                response.WriteByte(1);
                response.WriteByte(0);
                response.WriteByte(7);
                response.WriteByte(0);
                response.WriteByte((byte) client.Session.ChannelId); //1-xCH / 0=1ch 1=2ch 2=3ch 3=4ch
                Send(client, 5, response);

                if (client.Room != null)
                {
                    Room room = client.Room;
                    room.Leave(client);

                    IBuffer roomCharacterPacket = RoomPacket.CreateCharacterPacket(room);
                    Send(room, 10, roomCharacterPacket);

                    _logger.Debug("Character {0} left room {1}", client.Character.Name, room.Info.Name);
                }
                else
                {
                    _logger.Error("Character {0} left NULL room", client.Character.Name);
                }
                
                IBuffer announceRoomPacket = RoomPacket.CreateAnnounceRoomPacket(client.Channel);
                Send(client.Channel.GetLobbyClients(), 13, announceRoomPacket);

                IBuffer characterList = LobbyCharacterListPacket.Create(client.Channel);
                Send(client.Channel.GetLobbyClients(), 2, characterList);
            }
        }
    }
}