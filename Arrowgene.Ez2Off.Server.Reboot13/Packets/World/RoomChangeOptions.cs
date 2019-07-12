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
    public class RoomChangeOptions : Handler<WorldServer>
    {
        public RoomChangeOptions(WorldServer server) : base(server)
        {
        }

        public override int Id => 0x0A;

        public override void Handle(EzClient client, EzPacket packet)
        {
            client.Room.Info.PasswordProtected = packet.Data.ReadInt16() > 0;
            //byte PasswordProtected = packet.Data.ReadByte(); //비밀번호체크
            //packet.Data.ReadByte();
            packet.Data.ReadByte(); //6A
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            client.Room.Info.Password = packet.Data.ReadFixedString(4); // 1비밀번호 4BYTE
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            client.Room.Info.MaxPlayer = packet.Data.ReadByte(); // 최대 인원
            client.Room.Info.GameType = (GameType) packet.Data.ReadByte(); // 1
            client.Room.Info.GameGroupType = (GameGroupType) packet.Data.ReadByte(); // 게임 방식
            client.Room.Info.AllowViewer = packet.Data.ReadByte() > 0; // 관전 모드
            client.Room.Info.MaxDifficulty = (DifficultyType) packet.Data.ReadByte(); // 난이도 제한
            packet.Data.ReadByte(); // SELECTED SONG?
            client.Room.Info.Name = packet.Data.ReadFixedString(21, Utils.KoreanEncoding); // 방 이름 21 byte

            IBuffer Result = EzServer.Buffer.Provide();
            Result.WriteByte(0);
            Result.WriteByte(0);
            Result.WriteByte(0);
            Result.WriteByte(0);
            Result.WriteByte(0);
            Result.WriteByte(0);
            Result.WriteByte(0);
            Result.WriteByte(1); //항상 1로 설정
            Result.WriteByte((byte) client.Room.Info.GameGroupType); // 게임 방식
            Result.WriteByte(client.Room.Info.MaxPlayer); // 최대 인원
            Result.WriteByte(client.Room.Info.AllowViewer ? (byte) 1 : (byte) 0); // 관전모드
            Result.WriteByte((byte) client.Room.Info.MaxDifficulty); // 난이도 제한
            Result.WriteByte(0);
            Result.WriteFixedString(client.Room.Info.Name, 20, Utils.KoreanEncoding); // 방 이름 21byte
            Result.WriteByte(0);
            Send(client.Room, 0x11, Result);

            IBuffer announceRoomPacket = RoomPacket.CreateAnnounceRoomPacket(client.Channel);
            Send(client.Channel.GetLobbyClients(), 13, announceRoomPacket);

            client.Room.Log(_logger);
        }
    }
}