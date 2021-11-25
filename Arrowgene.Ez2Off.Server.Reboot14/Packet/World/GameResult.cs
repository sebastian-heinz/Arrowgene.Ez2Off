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

using System;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class GameResult : Handler<EzServer>
    {
        public GameResult(EzServer server) : base(server)
        {
        }

        public override int Id => 18;

        public override void Handle(EzClient client, EzPacket packet)
        {
            Score score = new Score();
            score.Character = client.Character;

            Room room = client.Room;

            if (room == null)
            {
                return;
            }

            Game game = room.Game;
            if (game == null)
            {
                room.FinishGame(client);
                return;
            }
            
            Song song = game.Song;
            if (song == null)
            {
                room.FinishGame(client);
                return;
            }
            
            score.Game = game;
            score.Song = song;
            score.Created = DateTime.Now;
            score.Difficulty = room.Difficulty;
            score.FadeEffect = room.FadeEffect;
            score.NoteEffect = room.NoteEffect;
            score.Mode = client.Mode;

            byte unknown0 = packet.Data.ReadByte();
            score.StageClear = packet.Data.ReadByte() == 0;
            short unknown1 = packet.Data.ReadInt16(Endianness.Big);
            score.MaxCombo = packet.Data.ReadInt16(Endianness.Big);
            score.Kool = packet.Data.ReadInt16(Endianness.Big);
            score.Cool = packet.Data.ReadInt16(Endianness.Big);
            score.Good = packet.Data.ReadInt16(Endianness.Big);
            score.Miss = packet.Data.ReadInt16(Endianness.Big);
            score.Fail = packet.Data.ReadInt16(Endianness.Big);
            short unknown2 = packet.Data.ReadInt16(Endianness.Big);
            score.RawScore = packet.Data.ReadInt32(Endianness.Big);
            score.TotalNotes = packet.Data.ReadInt16(Endianness.Big);
            score.Rank = (ScoreRankType) packet.Data.ReadByte();
            byte unknown3 = packet.Data.ReadByte();
            score.ComboType = Score.GetComboType(score);

            client.Score = score;
            room.FinishGame(client);
        }
    }
}