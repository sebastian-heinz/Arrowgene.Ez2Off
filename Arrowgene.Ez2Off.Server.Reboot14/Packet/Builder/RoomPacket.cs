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
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Packet.Builder;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder
{
    public class RoomPacket : IRoomPacket
    {
        private static readonly byte[] EmptySlot = new byte[666];

        private static readonly ILogger _logger = LogProvider.Logger(typeof(RoomPacket));
        
        
        public IBuffer CreateJoinErrorPacket()
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            return buffer;
        }

        public IBuffer CreateOpenRoomPacket(Room room, EzClient client)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteInt16(room.Number, Endianness.Big);
            buffer.WriteInt16((short) client.Player.Slot, Endianness.Big);
            buffer.WriteInt16((short) room.Master.Player.Slot, Endianness.Big);
            buffer.WriteByte(0);
            buffer.WriteByte((byte) (room.GameType));
            buffer.WriteByte((byte) room.GameGroupType);
            buffer.WriteByte(room.MaxPlayer);
            buffer.WriteByte(room.AllowViewer ? (byte) 1 : (byte) 0);
            buffer.WriteByte((byte) room.MaxDifficulty);
            buffer.WriteByte(0);
            buffer.WriteByte(room.RandomSong ? (byte) 1 : (byte) 0); //randomdisk
            buffer.WriteInt16((short) room.SelectedSong, Endianness.Big);
            if (room.MaxDifficulty == DifficultyType.None)
            {
                buffer.WriteByte(0); //Select difficulty  0=EZ 1=NM 2=HD 3=SHD
            }
            else
            {
                buffer.WriteByte((byte) room.MaxDifficulty);
            }

            buffer.WriteFixedString(room.Name, 21, Utils.KoreanEncoding);
            buffer.WriteByte(0);
            return buffer;
        }

        public IBuffer CreateJoinRoomPacket(Room room, EzClient client)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteInt16(room.Number, Endianness.Big);
            buffer.WriteInt16((short) client.Player.Slot, Endianness.Big);
            buffer.WriteInt16((short) room.Master.Player.Slot, Endianness.Big);
            buffer.WriteByte(0);
            buffer.WriteByte((byte) (room.GameType));
            buffer.WriteByte((byte) room.GameGroupType);
            buffer.WriteByte(room.MaxPlayer);
            buffer.WriteByte((byte) room.MaxDifficulty);
            buffer.WriteByte(0);
            buffer.WriteByte(room.RandomSong ? (byte) 1 : (byte) 0);
            buffer.WriteInt16((short) room.SelectedSong, Endianness.Big);
            buffer.WriteByte((byte) room.Difficulty);
            buffer.WriteByte(room.AllowViewer ? (byte) 1 : (byte) 0);
            buffer.WriteFixedString(room.Name, 21, Utils.KoreanEncoding);
            buffer.WriteByte(0);
            return buffer;
        }

        /// <summary>
        /// 룸 리스트 생성
        /// </summary>
        public IBuffer RoomList(Channel channel)
        {
            List<Room> rooms = channel.GetRooms();

            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte((byte) rooms.Count); // 현재 만들어진 방 개수
            //방 생성할때마다  패킷 추가

            foreach (Room room in rooms)
            {
                buffer.WriteInt16(room.Number, Endianness.Big); // 방 번호
                buffer.WriteByte(room.Playing ? (byte) 2 : (byte) 1); //Playing Byte
                buffer.WriteByte(room.MaxPlayer); // 최대 인원수
                buffer.WriteByte((byte) room.GetClients().Count); // 현재 참여한 인원수
                buffer.WriteByte((byte) room.GameType); // 0 = 싱글 1 = 멀티
                buffer.WriteByte((byte) room.GameGroupType); // team
                buffer.WriteByte(room.PasswordProtected ? (byte) 1 : (byte) 0);
                buffer.WriteByte((byte) room.MaxDifficulty); // Max
                buffer.WriteByte(0);
                buffer.WriteByte(room.RandomSong ? (byte) 1 : (byte) 0); // 1 = 랜덤 디스크
                buffer.WriteInt16((short) room.SelectedSong, Endianness.Big); // 디스크 번호
                buffer.WriteByte((byte) room.Difficulty); // 0 = 이지 1 = 노멀 2 = 하드
                buffer.WriteFixedString(room.Name, 21, Utils.KoreanEncoding);
                buffer.WriteByte(0);
            }

            return buffer;
        }

        public IBuffer UpdateRoomStatus(Room room)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            if (room == null)
            {
                // Null room should be closed and no need to update.
                _logger.Error("Tried to update 'null' room");
                return buffer;
            }

            buffer.WriteInt16(room.Number, Endianness.Big); // 방 번호
            buffer.WriteByte(room.Playing ? (byte) 2 : (byte) 1); //Playing Byte
            buffer.WriteByte(room.MaxPlayer); // 최대 인원수
            buffer.WriteByte((byte) room.GetClients().Count); // 현재 참여한 인원수
            buffer.WriteByte((byte) room.GameType); // 0 = 싱글 1 = 멀티
            buffer.WriteByte((byte) room.GameGroupType); // team
            buffer.WriteByte(room.PasswordProtected ? (byte) 1 : (byte) 0);
            buffer.WriteByte((byte) room.MaxDifficulty); // Max
            buffer.WriteByte(0);
            buffer.WriteByte(room.RandomSong ? (byte) 1 : (byte) 0); // 1 = 랜덤 디스크
            buffer.WriteInt16((short) room.SelectedSong, Endianness.Big); // 디스크 번호
            buffer.WriteByte((byte) room.Difficulty); // 0 = 이지 1 = 노멀 2 = 하드
            buffer.WriteFixedString(room.Name, 21, Utils.KoreanEncoding);
            buffer.WriteByte(0);
            return buffer;
        }

        public IBuffer KickPlayer(byte playerSlot)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            buffer.WriteByte(playerSlot);
            return buffer;
        }

        public IBuffer LeavePlayer(byte playerSlot)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            buffer.WriteByte(playerSlot);
            return buffer;
        }

        public IBuffer NewMaster(byte playerSlot)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            buffer.WriteByte(playerSlot);
            return buffer;
        }

        public IBuffer AnnounceJoin(EzClient client)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            WriteCharacter(buffer, client.Player.Slot, client);
            return buffer;
        }

        public IBuffer CreateCharacterPacket(Room room)
        {
            IBuffer buffer = EzServer.Buffer.Provide();


            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);


            for (int i = 0; i < Room.MaxSlots; i++)
            {
                EzClient client = room.GetClient(i);
                if (client != null)
                {
                    WriteCharacter(buffer, i, client);
                }
                else
                {
                    buffer.WriteByte(0xFF);
                    buffer.WriteByte(0xFF);
                    buffer.WriteByte(0xFF);
                    buffer.WriteByte(0xFF);

                    buffer.WriteBytes(EmptySlot);

                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                }
            }

            return buffer;
        }

        /// <summary>
        /// 0x41FAB0
        /// </summary>
        public void WriteCharacter(IBuffer buffer, int index, EzClient client)
        {
            Character character = client.Character;
            Inventory inventory = client.Inventory;
            Setting setting = client.Setting;

            buffer.WriteByte((byte) index); //Character index 1
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte((byte) client.Player.Ready);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte((byte) client.Player.Team); //TEAM 1=RED 2=BLUE
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
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteFixedString(character.Name, 18, Utils.KoreanEncoding);

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
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            //My LV
            buffer.WriteByte(character.Level);
            buffer.WriteByte(0);

            //Exr
            buffer.WriteInt16((short) character.RubyExr); //Ruby
            buffer.WriteInt16((short) character.StreetExr); //Street
            buffer.WriteInt16((short) character.ClubExr); //Club

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
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteInt16(character.DjPoints);
            buffer.WriteInt16(character.DjPointsPlus);

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
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            //Information
            //Highest score
            buffer.WriteInt32(character.RubyMaxScore); //Ruby
            buffer.WriteInt32(character.StreetMaxScore); //Street
            buffer.WriteInt32(character.ClubMaxScore); //Club
            buffer.WriteInt16(character.MaxCombo); //MaxCombo
            //Win
            buffer.WriteInt32(character.RubyWins); //RubyWins
            buffer.WriteInt32(character.StreetWins); //StreetWins
            buffer.WriteInt32(character.ClubWins); //ClubWins

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

            buffer.WriteByte(0); // VIP / 이지투온 PC방 혜택
            buffer.WriteByte(0); //전용 컨트롤러 Dedicated controller
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            int baseHp = client.Character.GetHp();
            int bonusHp = Character.CalculateItemHpGain(baseHp, client.Inventory.GetHpBonusPercentage());
            buffer.WriteInt16((short) (baseHp + bonusHp));

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

            buffer.WriteInt16((short) inventory.GetAvatarId()); //아바타 Avatar
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

            buffer.WriteInt16((short) inventory.GetSkinId()); //스킨 SKIN
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

            buffer.WriteInt16((short) inventory.GetNoteId()); //노트 NOTE
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

            buffer.WriteInt16((short) inventory.GetPremiumKeyId()); //프리미엄 열쇠(Premiumkey)
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteInt16((short) inventory.GetFreeTicketId()); //자유 이용권(FreeTicket)
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
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(setting.SkinType); //Onlinn or AcadeType Set (fucking T_T)
            buffer.WriteByte(0);
        }

        public IBuffer RoomClosed(Room room)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt16(room.Number, Endianness.Big); //방 번호
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            return buffer;
        }

        public IBuffer ItemUpdate(byte slot, Inventory inventory)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            buffer.WriteByte(slot);
            PacketBuilder.InventoryPacket.WriteEquipSlots(inventory, buffer);
            return buffer;
        }

        public IBuffer WaitingList(Channel channel)
        {
            List<EzClient> clients = channel.GetLobbyClients();
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt16((short) clients.Count, Endianness.Big);
            for (int i = 0; i < clients.Count; i++)
            {
                WriteWaitingList(buffer, clients[i].Character);
            }

            return buffer;
        }

        public void WriteWaitingList(IBuffer buffer, Character character)
        {
            buffer.WriteFixedString(character.Name, 18, Utils.KoreanEncoding);
            buffer.WriteByte(0);
            buffer.WriteByte(character.Level);
        }
    }
}