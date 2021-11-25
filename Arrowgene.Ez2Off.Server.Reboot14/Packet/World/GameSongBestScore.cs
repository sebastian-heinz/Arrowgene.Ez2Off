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
using Arrowgene.Buffers;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class GameSongBestScore : Handler<EzServer>
    {
        public GameSongBestScore(EzServer server) : base(server)
        {
        }

        public override int Id => 27;

        public override void Handle(EzClient client, EzPacket packet)
        {
            ModeType mode = (ModeType) packet.Data.ReadInt32();
            int songId = packet.Data.ReadInt32();
            DifficultyType difficulty = (DifficultyType) packet.Data.ReadInt32();

            client.BestScore = Database.SelectBestScore(client.Account.Id, songId, mode, difficulty);
            List<Score> scores = Database.SelectBestScores(songId, mode, difficulty, 5);
            scores.Sort((x, y) => y.TotalScore.CompareTo(x.TotalScore));

            IBuffer buffer = EzServer.Buffer.Provide();
            if (client.BestScore != null)
            {
                buffer.WriteInt32(client.BestScore.TotalScore);
                Logger.Info(client, $"BestScore Start:{client.BestScore.TotalScore}");
            }
            else
            {
                buffer.WriteInt32(0);
            }

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            for (int i = 0; i < 5; i++)
            {
                if (i >= scores.Count)
                {
                    buffer.WriteBytes(new byte[18]);
                    continue;
                }

                Score score = scores[i];
                if (score == null)
                {
                    buffer.WriteBytes(new byte[18]);
                    continue;
                }

                buffer.WriteFixedString(score.Character.Name, 18, Utils.KoreanEncoding);
            }

            for (int i = 0; i < 5; i++)
            {
                if (i >= scores.Count)
                {
                    buffer.WriteInt32(0);
                    continue;
                }

                Score score = scores[i];
                if (score == null)
                {
                    buffer.WriteInt32(0);
                    continue;
                }

                buffer.WriteInt32(score.TotalScore);
            }

            Router.Send(client, 38, buffer);
        }
    }
}