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
    public class RoomCreate : Handler<WorldServer>
    {
        public RoomCreate(WorldServer server) : base(server)
        {
        }

        public override int Id => 5;

        public override void Handle(EzClient client, EzPacket packet)
        {
            if (client.Room == null)
            {
                RoomInfo info = new RoomInfo();
                info.PasswordProtected = packet.Data.ReadInt16() > 0;
                packet.Data.ReadInt32(); // 69~6A
                info.Password = packet.Data.ReadFixedString(4, Utils.KoreanEncoding);
                packet.Data.ReadInt32();
                packet.Data.ReadInt32();
                info.MaxPlayer = packet.Data.ReadByte();
                info.GameType = (GameType) packet.Data.ReadByte(); // SinglePlayer / MultiPlayer
                info.GameGroupType = (GameGroupType) packet.Data.ReadByte(); // Individual / Team
                info.AllowViewer = packet.Data.ReadByte() > 0;
                info.MaxDifficulty = (DifficultyType) packet.Data.ReadByte();
                info.SelectedSong = packet.Data.ReadInt32(Endianness.Big);
                info.Name = packet.Data.ReadFixedString(20, Utils.KoreanEncoding);

                Room room = client.Channel.CreateRoom(info, client);

                IBuffer openRoomPacket = RoomPacket.CreateOpenRoomPacket(client.Room, client);
                Send(client, 9, openRoomPacket);
                
                IBuffer characterList = LobbyCharacterListPacket.Create(client.Channel);
                Send(client.Channel.GetLobbyClients(), 2, characterList);

                _logger.Debug("Opened Room");
            }
            else
            {
                //HEX:05-00-39-00-00-00-00-00-00-67-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-0A-
                //00-00-00-0E-C3-CA-BA-B8-B8-B8-20-BF-C0-BC-BC-BF-E4-21-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00

                //HEX:05-00-39-00-00-00-00-00-00-67-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-0A-
                //00-00-00-0E-C3-CA-BA-B8-B8-B8-20-BF-C0-BC-BC-BF-E4-21-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00


                IBuffer joinRoomPacket = RoomPacket.CreateJoinRoomPacket(client.Room, client);

                Send(client, 8, joinRoomPacket);

                _logger.Debug("Joined Room");
            }

            IBuffer roomCharacterPacket = RoomPacket.CreateCharacterPacket(client.Room);
            Send(client, 10, roomCharacterPacket);

            IBuffer announceRoomPacket = RoomPacket.CreateAnnounceRoomPacket(client.Channel);
            Send(client.Channel.GetLobbyClients(), 13, announceRoomPacket);

            client.Room.Log(_logger);
        }
    }
}