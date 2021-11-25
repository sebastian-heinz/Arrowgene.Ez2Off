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
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder
{
    public class GamePacket
    {
        private static readonly EzClient.ScoreComparer ScoreComparer = new EzClient.ScoreComparer(SortOrderType.DESC);

        public static IBuffer CreateScore(Room room)
        {
            List<EzClient> clients = room.GetClients();
            clients.Sort(ScoreComparer);

            List<EzClient> cleared = new List<EzClient>();
            List<EzClient> gameOver = new List<EzClient>();
            List<EzClient> watcher = new List<EzClient>();

            int blueScore = 0;
            int redScore = 0;

            byte currentRanking = 0;
            foreach (EzClient client in clients)
            {
                client.Rank = new Rank();
                client.Rank.Game = client.Room.Game;
                client.Rank.Score = client.Score;
                client.Rank.Team = client.Player.Team;

                if (client.Player.Ready != ReadyType.Ready)
                {
                    client.Rank.Ranking = 9;
                    watcher.Add(client);
                }
                else if (client.Score.StageClear)
                {
                    client.Character.IncreaseExr(client.Score);
                    client.Rank.Ranking = currentRanking;
                    cleared.Add(client);
                    currentRanking++;
                    if (client.Player.Team == TeamType.Red)
                    {
                        redScore += client.Score.TotalScore;
                    }
                    else if (client.Player.Team == TeamType.Blue)
                    {
                        blueScore += client.Score.TotalScore;
                    }
                }
                else
                {
                    client.Rank.Ranking = currentRanking;
                    gameOver.Add(client);
                    currentRanking++;
                    if (client.Player.Team == TeamType.Red)
                    {
                        redScore += client.Score.TotalScore;
                    }
                    else if (client.Player.Team == TeamType.Blue)
                    {
                        blueScore += client.Score.TotalScore;
                    }
                }
            }

            List<EzClient> ordered = new List<EzClient>();

            ordered.AddRange(cleared);
            ordered.AddRange(gameOver);
            ordered.AddRange(watcher);

            short count = (short) ordered.Count;
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt16((short) ordered.Count, Endianness.Big);
            for (short i = 0; i < count; i++)
            {
                EzClient client = ordered[i];
                Score score = client.Score;

                if (score != null)
                {
                    buffer.WriteInt16((byte) client.Player.Slot, Endianness.Big);
                    buffer.WriteByte(score.StageClear ? (byte) 0 : (byte) 1);
                    buffer.WriteInt16((short) score.MaxCombo, Endianness.Big);
                    buffer.WriteInt16((short) score.Kool, Endianness.Big);
                    buffer.WriteInt16((short) score.Cool, Endianness.Big);
                    buffer.WriteInt16((short) score.Good, Endianness.Big);
                    buffer.WriteInt16((short) score.Miss, Endianness.Big);
                    buffer.WriteInt16((short) score.Fail, Endianness.Big);
                    buffer.WriteByte(0);
                    buffer.WriteByte((byte) score.ComboType);
                    buffer.WriteInt32(score.TotalScore, Endianness.Big);
                    buffer.WriteInt16(0, Endianness.Big); // + EXP
                    buffer.WriteByte((byte) score.Rank);
                    buffer.WriteInt16(0, Endianness.Big); // + Coin Increase [MAX:9999]
                    buffer.WriteByte(0); // 1 = Level Up [HP Points +1 / DJ Points +1] increase
                    buffer.WriteInt16((short) score.TotalNotes, Endianness.Big);

                    if (room.GameGroupType == GameGroupType.Individual)
                    {
                        buffer.WriteByte(client.Rank.Ranking); // Ranking 0 = 1st, 2 = 2nd, .. 7 = 8th, 9 = Watch
                        buffer.WriteByte(client.Rank.Ranking == 0 ? (byte) 0 : (byte) 1); // loose =1 /  win  = 0 ?? 
                    }
                    else
                    {
                        bool win = false;
                        if (client.Player.Team == TeamType.Blue)
                        {
                            win = blueScore >= redScore;
                        }
                        else if (client.Player.Team == TeamType.Red)
                        {
                            win = redScore >= blueScore;
                        }

                        if (!score.StageClear)
                        {
                            win = false;
                        }

                        buffer.WriteByte(win ? (byte) 0 : (byte) 1);
                        buffer.WriteByte(win ? (byte) 0 : (byte) 1);
                    }

                    buffer.WriteInt16(0); //EXP +%
                    buffer.WriteInt16(0); //Coin +%
                    buffer.WriteByte(client.Character.Level);
                    buffer.WriteInt32(client.Character.Exp);
                    buffer.WriteInt32(Character.ExpForNextLevel(client.Character));
                    if (client.BestScore != null && client.BestScore.TotalScore > score.TotalScore)
                    {
                        buffer.WriteInt32(client.BestScore.TotalScore, Endianness.Big);
                    }
                    else
                    {
                        buffer.WriteInt32(score.TotalScore, Endianness.Big);
                    }

                    buffer.WriteByte((byte) client.Character.GetExr(score.Mode)); // Song completion EXR increase
                    buffer.WriteByte(0);
                }
                else
                {
                    // Watcher - Has no Score
                    buffer.WriteInt16((byte) client.Player.Slot, Endianness.Big);
                    buffer.WriteByte(1); // Stage Clear
                    buffer.WriteInt16(0, Endianness.Big); // MaxCombo
                    buffer.WriteInt16(0, Endianness.Big); // Kool
                    buffer.WriteInt16(0, Endianness.Big); // Cool
                    buffer.WriteInt16(0, Endianness.Big); // Good
                    buffer.WriteInt16(0, Endianness.Big); // Miss
                    buffer.WriteInt16(0, Endianness.Big); // Fail
                    buffer.WriteByte(0);
                    buffer.WriteByte((byte) ComboType.None);
                    buffer.WriteInt32(0); // TotalScore
                    buffer.WriteInt16(0); // + EXP
                    buffer.WriteByte((byte) ScoreRankType.F);
                    buffer.WriteInt16(0, Endianness.Big); // + Coin Increase [MAX:9999]
                    buffer.WriteByte(0); // 1 = Level Up [HP Points +1 / DJ Points +1] increase
                    buffer.WriteInt16(0, Endianness.Big); // TotalNotes
                    buffer.WriteByte(9); // Ranking 0 = 1st, 2 = 2nd, .. 7 = 8th, 9 = Watch
                    buffer.WriteByte(0); // loose = 1 /  win  = 0
                    buffer.WriteInt16(0); //EXP +%
                    buffer.WriteInt16(0); //Coin +%
                    buffer.WriteByte(client.Character.Level);
                    buffer.WriteInt32(client.Character.Exp);
                    buffer.WriteInt32(Character.ExpForNextLevel(client.Character));
                    if (client.BestScore != null)
                    {
                        buffer.WriteInt32(client.BestScore.TotalScore, Endianness.Big);
                    }
                    else
                    {
                        buffer.WriteInt32(0, Endianness.Big);
                    }

                    buffer.WriteByte((byte) client.Character.GetExr(client.Mode)); // Song completion EXR increase
                    buffer.WriteByte(0);
                }
            }


            return buffer;
        }

        public static IBuffer CreateGameOver(byte slot)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            buffer.WriteByte(slot);
            buffer.WriteByte(1);
            return buffer;
        }

        public static IBuffer CreateGameStart(Song song, ModeType mode, DifficultyType difficulty)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            if (song == null)
            {
                buffer.WriteBytes(new byte[28]);
                buffer.WriteByte(0); // Can Start Game? 0 = False | 1 = true 
                return buffer;
            }

            buffer.WriteByte((byte) song.GetExr(mode, difficulty));
            buffer.WriteFloat(song.MeasureScale);
            buffer.WriteByte(song.JudgmentKool);
            buffer.WriteByte(song.JudgmentCool);
            buffer.WriteByte(song.JudgmentGood);
            buffer.WriteByte(song.JudgmentMiss);
            buffer.WriteFloat(song.GaugeCool);
            buffer.WriteFloat(song.GaugeGood);
            buffer.WriteFloat(song.GaugeMiss);
            buffer.WriteFloat(song.GaugeFail);
            buffer.WriteInt16((short) song.Id, Endianness.Big);
            buffer.WriteByte((byte) difficulty);
            buffer.WriteByte(1); // Can Start Game? 0 = False | 1 = true 
            return buffer;
        }
    }
}