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
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder
{
    public class RoomPacket
    {
        private static byte[] EmptySlot = new byte[356];

        public static IBuffer CreateJoinErrorPacket()
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            return buffer;
        }

        public static IBuffer CreateOpenRoomPacket(Room room, EzClient client)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteByte(0x6A);
            buffer.WriteByte(0);
            buffer.WriteByte(room.Info.Number); //Room number
            buffer.WriteByte(0);
            buffer.WriteByte(room.Master == client ? (byte) 0 : (byte) 1); // 0=master 1=user
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte((byte) (room.Info.GameType)); // 0=Single 1=Multi
            buffer.WriteByte((byte) room.Info.GameGroupType); // 1 = Team Competition
            buffer.WriteByte(room.Info.MaxPlayer); // open slot max 1~8
            buffer.WriteByte(0);
            buffer.WriteByte((byte) room.Info.MaxDifficulty);
            buffer.WriteByte(0);
            buffer.WriteByte(0); // 1 = 랜덤 디스크 // random
            buffer.WriteByte(0);
            buffer.WriteByte((byte) room.Info.SelectedSong); //Disc Num
            buffer.WriteByte((byte) room.Info.Difficulty); //Select difficulty  0=EZ 1=NM 2=HD 3=SHD
            buffer.WriteFixedString(room.Info.Name, 20, Utils.KoreanEncoding);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            return buffer;
        }

        public static IBuffer CreateJoinRoomPacket(Room room, EzClient client)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteByte(0x6A);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0); //
            buffer.WriteByte(room.Master == client ? (byte) 0 : (byte) 1); // 0=master 1=user
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte((byte) (room.Info.GameType)); // 0=Single 1=Multi
            buffer.WriteByte((byte) room.Info.GameGroupType); // 1 = Team Competition
            buffer.WriteByte(room.Info.MaxPlayer); // open slot max 1~8
            buffer.WriteByte((byte) room.Info.MaxDifficulty);
            buffer.WriteByte(0);
            buffer.WriteByte(0); // 1 = 랜덤 디스크 // random
            buffer.WriteByte(0);
            buffer.WriteByte((byte) room.Info.SelectedSong);
            buffer.WriteByte((byte) room.Info.Difficulty);
            buffer.WriteByte(0);
            buffer.WriteFixedString(room.Info.Name, 20, Utils.KoreanEncoding);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            return buffer;
        }


        public static IBuffer AnnounceJoin(EzClient client)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            WriteCharacter(buffer, client.Player.Slot, client);
            return buffer;
        }

        public static IBuffer CreateCharacterPacket(Room room)
        {
            IBuffer buffer = EzServer.Buffer.Provide();


            buffer.WriteByte(2);
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
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);

                    buffer.WriteBytes(EmptySlot);

                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                }
            }

            return buffer;
        }

        public static void WriteCharacter(IBuffer buffer, int index, EzClient client)
        {
            Character character = client.Character;
            Inventory inventory = client.Inventory;


            buffer.WriteByte((byte) index);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(client.Player.Ready ? (byte) 1 : (byte) 0); // Ready
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte((byte) client.Player.Team); //TEAM 1=RED 2=BLUE
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteFixedString(character.Name, 16, Utils.KoreanEncoding);
            buffer.WriteByte(0); // String Termination
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte((byte) character.Sex);
            buffer.WriteByte(0);
            buffer.WriteByte(character.Level);
            buffer.WriteByte(0);

            // Exr
            buffer.WriteInt16((short) character.RubyExr); //Ruby Mix Exr
            buffer.WriteInt16((short) character.StreetExr); //Street Mix Exr
            buffer.WriteInt16((short) character.ClubExr); //Club Mix Exr
            buffer.WriteInt16((short) character.GetExr(client.Mode)); //My Exr

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
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
            buffer.WriteInt32(0); //Ruby Mix
            buffer.WriteInt32(0); //Street Mix
            buffer.WriteInt32(0); //Club Mix
            buffer.WriteInt16(character.MaxCombo); //Max Combo

            //Record
            buffer.WriteInt32(character.RubyWins); //Ruby Wins
            buffer.WriteInt32(character.StreetWins); //Street Wins
            buffer.WriteInt32(character.ClubWins); //Club Wins

            buffer.WriteInt32(character.RubyLoses); //Ruby Lose
            buffer.WriteInt32(character.StreetLoses); //Street Lose
            buffer.WriteInt32(character.ClubLoses); //Club Lose

            //Premium
            buffer.WriteByte(0); //1
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(0x64); // HP 100
            buffer.WriteByte(0);

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteInt16((short) inventory.GetAvatarId());
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteInt16((short) inventory.GetSkinId());
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteInt16((short) inventory.GetNoteId());
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);


            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
        }


        /// <summary>
        /// 룸 리스트 생성
        /// </summary>
        public static IBuffer CreateAnnounceRoomPacket(Channel channel)
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
                buffer.WriteInt16(room.Info.Number, Endianness.Big); // 방 번호
                buffer.WriteByte(0);
                buffer.WriteByte(room.Info.MaxPlayer); // 최대 인원수
                buffer.WriteByte((byte) room.GetClients().Count); // 현재 참여한 인원수
                buffer.WriteByte((byte) room.Info.GameType); // 0 = 싱글 1 = 멀티
                buffer.WriteByte((byte) room.Info.GameGroupType); // team
                buffer.WriteByte(room.Info.PasswordProtected ? (byte) 1 : (byte) 0);
                buffer.WriteByte((byte) room.Info.Difficulty); // 0 = 이지 1 = 노멀 2 = 하드
                buffer.WriteByte(0); // 0 = Easy Score // 1 = Easy KOOL
                buffer.WriteByte(room.Info.RandomSong ? (byte) 1 : (byte) 0); // 1 = 랜덤 디스크
                buffer.WriteByte(0); // Maybe selected song big endian int 16 ?
                buffer.WriteByte((byte) room.Info.SelectedSong); // 디스크 번호
                buffer.WriteByte(0);
                buffer.WriteFixedString(room.Info.Name, 21, Utils.KoreanEncoding);
            }

            return buffer;
        }
    }
}