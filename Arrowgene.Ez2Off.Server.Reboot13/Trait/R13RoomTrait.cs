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
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder;
using Arrowgene.Ez2Off.Server.Trait;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Trait
{
    public class R13RoomTrait : RoomTrait
    {
        public R13RoomTrait(EzServer server) : base(server)
        {
        }

        public override void ClientLeave(Room room, EzClient client)
        {
            Router.Send(client, 5, LobbyPacket.CreateGoToLobby(client.ChannelIndex, (byte) client.Session.ChannelId));
            Router.Send(room, 12, RoomPacket.LeavePlayer((byte) client.Player.Slot));
            Router.Send(client, 13, RoomPacket.RoomList(client.Channel));
            Router.Send(client.Channel.GetLobbyClients(), 14, RoomPacket.UpdateRoomStatus(room), client);

            Router.Send(client, 2, LobbyPacket.CreateCharacterList(client.Channel));
            IBuffer characterListAdd = LobbyPacket.CreateCharacterListAdd(client);
            Router.Send(client.Channel.GetLobbyClients(), 3, characterListAdd, client);
        }

        public override void ClientJoin(Room room, EzClient client)
        {
            Router.Send(client, 8, RoomPacket.CreateJoinRoomPacket(room, client));
            Router.Send(room, 11, RoomPacket.AnnounceJoin(client), client);
            Router.Send(client, 10, RoomPacket.CreateCharacterPacket(room));
            Router.Send(client.Channel.GetLobbyClients(), 14, RoomPacket.UpdateRoomStatus(client.Room));

            IBuffer characterListRemove = LobbyPacket.CreateCharacterListRemove(client);
            Router.Send(client.Channel.GetLobbyClients(), 4, characterListRemove);
        }

        public override void NewMaster(Room room, EzClient client)
        {
            Router.Send(room, 15, RoomPacket.NewMaster((byte) room.Master.Player.Slot));
        }

        public override void Kick(Room room, EzClient client, byte playerSlot)
        {
            Router.Send(room, 22, RoomPacket.KickPlayer(playerSlot));
        }

        public override void GameStarting(Room room)
        {
        }

        public override void GameStart(Room room)
        {
            Router.Send(room.Channel.GetLobbyClients(), 14, RoomPacket.UpdateRoomStatus(room));
            Router.Send(room, 0x17, GamePacket.CreateGameStart(room.Game.Song, room.Mode, room.Difficulty));
        }

        public override void GameResult(Room room)
        {
            IBuffer scorePacket = GamePacket.CreateScore(room);
            Router.Send(room, 0x1B, scorePacket); //27

            if (!Server.Database.InsertGame(room.Game))
            {
                Logger.Error("Could not save game");
            }

            List<EzClient> clients = room.GetClients();
            foreach (EzClient client in clients)
            {
                if (client.Score != null && client.Rank != null)
                {
                    if (!Server.Database.InsertScore(client.Score))
                    {
                        Logger.Error(client, "Couldn't save score");
                    }

                    if (!Server.Database.InsertRank(client.Rank))
                    {
                        Logger.Error(client, "Couldn't save rank");
                    }
                }
            }
        }

        public override void ClientFinish(Room room, EzClient client)
        {
        
        }

        public override void GameFinish(Room room)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            Router.Send(room, 0x1C, buffer); //28
            Router.Send(room.Channel.GetLobbyClients(), 14, RoomPacket.UpdateRoomStatus(room));
        }

        public override void LoadingFinish(Room room)
        {
        }

        public override void NextRadiomixSong(Room room, EzClient client)
        {
        }

        public override void ChangeGameSetting(Room room, short playerSlot, RoomOptionType roomOption, int valueA = 0,
            int valueB = 0)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt16(playerSlot, Endianness.Big);
            buffer.WriteInt32((int) roomOption);
            buffer.WriteInt32(valueA);
            buffer.WriteInt32(valueB);

            Router.Send(room, 16, buffer);
            Router.Send(room.Channel.GetLobbyClients(), 14, RoomPacket.UpdateRoomStatus(room));
        }
    }
}