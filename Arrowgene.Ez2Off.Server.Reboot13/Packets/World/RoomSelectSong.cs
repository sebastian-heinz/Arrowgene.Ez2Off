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

using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Models;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class RoomSelectSong : Handler<WorldServer>
    {
        public RoomSelectSong(WorldServer server) : base(server)
        {
        }

        public override int Id => 9;

        public override void Handle(EzClient client, EzPacket packet)
        {
            RoomOptionType roomOption = (RoomOptionType) packet.Data.ReadInt32();
            _logger.Debug("Change Option: {0}", roomOption);
            IBuffer buffer = EzServer.Buffer.Provide();
            switch (roomOption)
            {
                case RoomOptionType.ChangeReady:
                {
                    bool ready = packet.Data.ReadInt32() > 0;
                    int unknown1A = packet.Data.ReadInt32();
                    _logger.Debug("ready: {0}", ready);
                    _logger.Debug("unknown1A: {0}", unknown1A); // Slot?
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteInt32((int) roomOption);
                    buffer.WriteInt32(ready ? 1 : 0);
                    buffer.WriteInt32(unknown1A);
                    client.Player.Ready = ready;
                    IBuffer roomCharacterPacket = RoomPacket.CreateCharacterPacket(client.Room);
                    Send(client.Room, 10, roomCharacterPacket);
                    break;
                }
                case RoomOptionType.ChangeTeam:
                {
                    TeamType team = (TeamType) packet.Data.ReadInt32();
                    int unknown0B = packet.Data.ReadInt32();
                    _logger.Debug("unknown0B: {0}", unknown0B);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteInt32((int) roomOption);
                    buffer.WriteInt32((int) team);
                    buffer.WriteInt32(unknown0B);
                    client.Player.Team = team;
                    IBuffer roomCharacterPacket = RoomPacket.CreateCharacterPacket(client.Room);
                    Send(client.Room, 10, roomCharacterPacket);
                    break;
                }
                case RoomOptionType.ChangeFade:
                    client.Room.Info.FadeEffect = (FadeEffectType) packet.Data.ReadInt32();
                    int unknown0C = packet.Data.ReadInt32();
                    _logger.Debug("unknown0C: {0}", unknown0C);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteInt32((int) roomOption);
                    buffer.WriteInt32((int) client.Room.Info.FadeEffect);
                    buffer.WriteInt32(unknown0C);
                    break;
                case RoomOptionType.ChangeNote:
                    client.Room.Info.NoteEffect = (NoteEffectType) packet.Data.ReadInt32();
                    int unknown0D = packet.Data.ReadInt32();
                    _logger.Debug("unknown0D: {0}", unknown0D);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteInt32((int) roomOption);
                    buffer.WriteInt32((int) client.Room.Info.NoteEffect);
                    buffer.WriteInt32(unknown0D);
                    break;
                case RoomOptionType.ChangeSongAndDifficulty:
                    client.Room.Info.RandomSong = false;
                    client.Room.Info.SelectedSong = packet.Data.ReadInt32();
                    client.Room.Info.Difficulty = (DifficultyType) packet.Data.ReadInt32();
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteInt32((int) roomOption);
                    buffer.WriteInt32((int) client.Room.Info.SelectedSong);
                    buffer.WriteInt32((int) client.Room.Info.Difficulty);
                    break;
                case RoomOptionType.StartGame:
                    int unknown0E = packet.Data.ReadInt32();
                    int unknown1E = packet.Data.ReadInt32();
                    _logger.Debug("unknown0E: {0}", unknown0E);
                    _logger.Debug("unknown1E: {0}", unknown1E);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteInt32((int) roomOption);
                    buffer.WriteInt32(unknown0E);
                    buffer.WriteInt32(unknown1E);
                    break;
                case RoomOptionType.ChangeRandom:
                    client.Room.Info.RandomSong = true;
                    client.Room.Info.SelectedSong = packet.Data.ReadInt32();
                    client.Room.Info.Difficulty = (DifficultyType) packet.Data.ReadInt32();
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteInt32((int) roomOption);
                    buffer.WriteInt32((int) client.Room.Info.SelectedSong);
                    buffer.WriteInt32((int) client.Room.Info.Difficulty);
                    break;
                case RoomOptionType.ViewVideo:
                    // TODO deduct 1000 coins.
                    break;
            }

            Send(client.Room, 16, buffer);

            IBuffer announceRoomPacket = RoomPacket.CreateAnnounceRoomPacket(client.Channel);
            Send(client.Channel.GetLobbyClients(), 13, announceRoomPacket);

            client.Room.Log(_logger);
        }
    }
}