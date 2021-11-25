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
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class RoomChangeOptions : Handler<EzServer>
    {
        public RoomChangeOptions(EzServer server) : base(server)
        {
        }

        public override int Id => 10;

        public override void Handle(EzClient client, EzPacket packet)
        {
            client.Room.PasswordProtected = packet.Data.ReadInt16() > 0;
            byte Unknown = packet.Data.ReadByte(); //6x
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte(); //client room number
            client.Room.Password = packet.Data.ReadFixedString(4); // 비밀번호 4BYTE
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            packet.Data.ReadByte();
            client.Room.MaxPlayer = packet.Data.ReadByte(); // 최대 인원
            client.Room.GameType = (GameType) packet.Data.ReadByte(); // 1
            client.Room.GameGroupType = (GameGroupType) packet.Data.ReadByte(); // 게임 방식
            client.Room.AllowViewer = packet.Data.ReadByte() > 0; // 관전 모드
            client.Room.MaxDifficulty = (DifficultyType) packet.Data.ReadByte(); // 난이도 제한
            packet.Data.ReadByte(); // SELECTED SONG?
            client.Room.Name = packet.Data.ReadFixedString(21, Utils.KoreanEncoding); // 방 이름 21 byte

            client.Room.ChangeRoomOptions();

            IBuffer Result = EzServer.Buffer.Provide();
            Result.WriteByte(0);
            Result.WriteByte(0);
            Result.WriteByte(0);
            Result.WriteByte(client.Room.Number);//Number
            Result.WriteByte(0);
            Result.WriteByte(0);
            Result.WriteByte(0);
            Result.WriteByte(1); //항상 1로 설정
            Result.WriteByte((byte) client.Room.GameGroupType); // 게임 방식
            Result.WriteByte(client.Room.MaxPlayer); // 최대 인원
            Result.WriteByte(client.Room.AllowViewer ? (byte) 1 : (byte) 0); // 관전모드
            Result.WriteByte((byte) client.Room.MaxDifficulty); // 난이도 제한
            Result.WriteByte(0);
            Result.WriteFixedString(client.Room.Name, 21, Utils.KoreanEncoding); // 방 이름 21byte
            Router.Send(client.Room, 17, Result);

            Router.Send(client.Channel.GetLobbyClients(), 14,  PacketBuilder.RoomPacket.UpdateRoomStatus(client.Room));
        }
    }
}