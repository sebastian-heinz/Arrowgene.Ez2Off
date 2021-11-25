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
using Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.World
{
    /// <summary>
    /// 1) Create new Multiplayer/Singleplayer room
    /// 2) Abort Singleplay (Press ESC)
    /// </summary>
    public class RoomCreate : Handler<EzServer>
    {
        public RoomCreate(EzServer server) : base(server)
        {
        }

        public override int Id => 5;

        public override void Handle(EzClient client, EzPacket packet)
        {
            if (client.Room != null)
            {
                if (client.Room.GameType == GameType.SinglePlayer)
                {
                    client.Room.SetPlaying(false);
                    IBuffer joinRoomPacket = RoomPacket.CreateJoinRoomPacket(client.Room, client);
                    Router.Send(client, 8, joinRoomPacket);

                    IBuffer roomCharacterPacket = RoomPacket.CreateCharacterPacket(client.Room);
                    Router.Send(client, 10, roomCharacterPacket);

                    IBuffer announceRoomPacket = RoomPacket.UpdateRoomStatus(client.Room);
                    Router.Send(client.Channel.GetLobbyClients(), 14, announceRoomPacket);

                    return;
                }

                Logger.Error(client, $"Room: [{client.Room.Number}]{client.Room.Name} not cleaned up");
            }

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
            info.Mode = client.Mode;
            client.Channel.CreateRoom(info, client);
        }
    }
}