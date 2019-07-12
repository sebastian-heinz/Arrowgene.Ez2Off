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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class RoomEntry : Handler<WorldServer>
    {
        public RoomEntry(WorldServer server) : base(server)
        {
        }

        public override int Id => 6;
        // 06-00-15-00-00-00-00-     00-00- 00-6A- 00-00-01- 00-00-00-00- 00-00-00-00-00-00-00-00-01-00
        // 6A ?
        // 01 Room number

        //HEX:06-00-15-00-00-00-00-  00-00- 00-67- 00-00-03 -00-00-00-00 -00-00-00-00-00-00-00-00-01-00

        //ASCII:.  .  .  .  .  .  .  .  .  .  g  .  .  .      s  d  d  d  .  .  .  .  .  .  .  .  .  .
        //HEX:06-00-15-00-00-00-00-  00- 01- 00-67 -00-00-00- 73-64-64-64- 00-00-00-00-00-00-00-00-01-00
        
        //    06-00-15-00-00-00-00- 00-01-00-67- 00-00-00-71-77-65-00-00-00-00-00-00-00-00-00-01-00
        //    06-00-15-00-00-00-00- 00-00-00-67- 00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-01-00


        
        // 12 = exit room TODO
        // 11 = join room
        
        
        public override void Handle(EzClient client, EzPacket packet)
        {
            byte quickStart = packet.Data.ReadByte();
            byte passwordProtected = packet.Data.ReadByte();
            byte unknown0 = packet.Data.ReadByte();
            byte channel = packet.Data.ReadByte(); // Channel + 100, 0 Indexed
            byte unknown1 = packet.Data.ReadByte();
            byte unknown2 = packet.Data.ReadByte();
            byte roomNumber = packet.Data.ReadByte();
            string password = packet.Data.ReadFixedString(4, Utils.KoreanEncoding);
            
            
            _logger.Debug("Channel: {0}, Password {1}, Room#:{2}, QuickStart:{3}, PwProtected:{4}", 
                channel - 100 + 1, password, roomNumber, quickStart, passwordProtected);

            Room room;
            if (quickStart > 0)
            {
                room = client.Channel.GetQuickRoom();
                if (room == null)
                {
                    _logger.Debug("No QuickStart Room found");
                    IBuffer joinErrorPacket = RoomPacket.CreateJoinErrorPacket();
                    Send(client, 8, joinErrorPacket);
                    return;
                }
            }
            else
            {
                room  = client.Channel.GetRoom(roomNumber);
            }

            if (room == null)
            {
                _logger.Error("Invalid room");
                IBuffer joinErrorPacket = RoomPacket.CreateJoinErrorPacket();
                Send(client, 8, joinErrorPacket);
                return;
            }

            if (passwordProtected > 0 && room.Info.Password != password)
            {
                _logger.Error("Invalid password ({0}) for room with password ({1})", password, room.Info.Password);
                IBuffer joinErrorPacket = RoomPacket.CreateJoinErrorPacket();
                Send(client, 8, joinErrorPacket);
                return;
            }
            
            room.Join(client);

            IBuffer joinRoomPacket = RoomPacket.CreateJoinRoomPacket(room, client);
            Send(client, 8, joinRoomPacket);

            IBuffer roomCharacterPacket = RoomPacket.CreateCharacterPacket(room);
            Send(room, 10, roomCharacterPacket);
            
            IBuffer announceJoinPacket = RoomPacket.AnnounceJoin(client);
            Send(room, 11, announceJoinPacket);

            IBuffer announceRoomPacket = RoomPacket.CreateAnnounceRoomPacket(client.Channel);
            Send(client.Channel.GetLobbyClients(), 13, announceRoomPacket);
            
            IBuffer characterList = LobbyCharacterListPacket.Create(client.Channel);
            Send(client.Channel.GetLobbyClients(), 2, characterList);

            _logger.Debug("Character {0} joined room {1}", client.Character.Name, room.Info.Name);
        }
    }
}