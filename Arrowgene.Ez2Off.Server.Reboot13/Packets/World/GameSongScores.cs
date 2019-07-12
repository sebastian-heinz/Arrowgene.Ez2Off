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
using System.Threading;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class GameSongScores : Handler<WorldServer>
    {
        public GameSongScores(WorldServer server) : base(server)
        {
        }

        public override int Id => 0x1C;

        public override void Handle(EzClient client, EzPacket packet)
        {
            int songId = client.Room.Info.SelectedSong;
            DifficultyType difficulty = client.Room.Info.Difficulty;
            Score myBestScore = Database.SelectBestScore(client.Account.Id, songId, difficulty);
            List<Score> scores = Database.SelectBestScores(songId, difficulty, 5);
            scores.Sort((x, y) => y.TotalScore.CompareTo(x.TotalScore));

            IBuffer buffer = EzServer.Buffer.Provide();
            if (myBestScore != null)
            {
                buffer.WriteInt32(myBestScore.TotalScore, Endianness.Big);
            }
            else
            {
                buffer.WriteInt32(0, Endianness.Big);
            }

            buffer.WriteByte(0);

            foreach (Score score in scores)
            {
                buffer.WriteFixedString(score.CharacterName, 17, Utils.KoreanEncoding);
                buffer.WriteInt32(score.TotalScore, Endianness.Big);
                buffer.WriteByte(0);
            }

            Send(client, 0x27, buffer);
        }
    }
}