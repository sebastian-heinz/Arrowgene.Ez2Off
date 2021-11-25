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
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.World
{
    public class GameSongScores : Handler<EzServer>
    {
        public GameSongScores(EzServer server) : base(server)
        {
        }

        public override int Id => 0x1C;

        public override void Handle(EzClient client, EzPacket packet)
        {
            int songId = client.Room.SelectedSong;
            DifficultyType difficulty = client.Room.Difficulty;
            ModeType mode = client.Room.Mode;
            client.BestScore = Database.SelectBestScore(client.Account.Id, songId, mode, difficulty);
            List<Score> scores = Database.SelectBestScores(songId, mode, difficulty, 5);
            scores.Sort((x, y) => y.TotalScore.CompareTo(x.TotalScore));

            IBuffer buffer = EzServer.Buffer.Provide();
            if (client.BestScore != null)
            {
                buffer.WriteInt32(client.BestScore.TotalScore, Endianness.Big);
            }
            else
            {
                buffer.WriteInt32(0, Endianness.Big);
            }

            buffer.WriteByte(0);

            foreach (Score score in scores)
            {
                buffer.WriteFixedString(score.Character.Name, 17, Utils.KoreanEncoding);
                buffer.WriteInt32(score.TotalScore, Endianness.Big);
                buffer.WriteByte(0);
            }

            Router.Send(client, 0x27, buffer);
        }
    }
}