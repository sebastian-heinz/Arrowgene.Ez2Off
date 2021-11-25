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
using Arrowgene.Ez2Off.Server.Packet.Builder;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder
{
    public class GamePacket : IGamePacket
    {
        private static readonly EzClient.ScoreComparer ScoreComparer = new EzClient.ScoreComparer(SortOrderType.DESC);

        public IBuffer CreateScore(Room room)
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
                else if (client.Score.Incident)
                {
                    client.Rank.Ranking = currentRanking;
                    gameOver.Add(client);
                    currentRanking++;
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


            buffer.WriteByte((byte) ordered.Count);
            for (short i = 0; i < count; i++)
            {
                EzClient client = ordered[i];
                Score score = client.Score;


                if (score != null)
                {
                    int baseExp = Character.CalculateExpGain(score);
                    int baseCoin = Character.CalculateCoinGain(score);
                    int itemExp = Character.CalculateItemExpGain(baseExp, client.Inventory.GetExpBonusPercentage());
                    int itemCoin = Character.CalculateItemCoinGain(baseCoin, client.Inventory.GetCoinBonusPercentage());
                    int totalExp = baseExp + itemExp;
                    int totalCoin = baseCoin + itemCoin;
                    if (client.Character.Level >= Character.MaxLevel)
                    {
                        totalExp = 0;
                    }

                    short maxCombo = (short) score.MaxCombo;
                    short kool = (short) score.Kool;
                    short cool = (short) score.Cool;
                    short good = (short) score.Good;
                    short miss = (short) score.Miss;
                    short fail = (short) score.Fail;
                    bool stageClear = score.StageClear;
                    ComboType comboType = score.ComboType;
                    int totalScore = score.TotalScore;
                    ScoreRankType rank = score.Rank;
                    short totalNotes = (short) score.TotalNotes;


                    if (score.Incident)
                    {
                        totalExp = 0;
                        totalCoin = 0;
                        maxCombo = 0;
                        kool = 0;
                        cool = 0;
                        good = 0;
                        miss = 0;
                        fail = 0;
                        stageClear = false;
                        comboType = ComboType.None;
                        totalScore = 0;
                        rank = ScoreRankType.F;
                        totalNotes = 0;
                    }
                    else
                    {
                        client.Character.IncreaseMaxCombo(client.Score);
                    }

                    client.Character.AddExp(totalExp);
                    client.Character.AddCoin(totalCoin);
                    bool levelUp = client.Character.IsLevelUp();

                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte((byte) client.Player.Slot);
                    buffer.WriteByte(0);
                    buffer.WriteByte(stageClear ? (byte) 0 : (byte) 1);
                    buffer.WriteInt16(maxCombo);
                    buffer.WriteInt16(kool);
                    buffer.WriteInt16(cool);
                    buffer.WriteInt16(good);
                    buffer.WriteInt16(miss);
                    buffer.WriteInt16(fail);
                    buffer.WriteByte((byte) comboType);
                    buffer.WriteByte(0);
                    buffer.WriteInt32(totalScore);
                    buffer.WriteInt32(totalExp);
                    buffer.WriteByte((byte) rank);
                    buffer.WriteInt16((short) totalCoin);
                    buffer.WriteInt16(client.Character.DjPoints);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteInt16(client.Character.DjPointsPlus);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(levelUp ? (byte) 1 : (byte) 0);
                    buffer.WriteInt16(totalNotes);

                    if (score.Incident)
                    {
                        buffer.WriteByte(room.GameGroupType == GameGroupType.Individual
                            ? (byte) client.Rank.Ranking
                            : (byte) 1
                        );
                        buffer.WriteByte(1); // You Loose
                    }
                    else if (room.GameGroupType == GameGroupType.Individual)
                    {
                        buffer.WriteByte(client.Rank.Ranking); // Ranking (0=1st | 1=2nd | .. | 7=8th | 9=Watch)
                        int playerCount = cleared.Count + gameOver.Count;
                        int minWinRank = playerCount / 2;
                        bool win = client.Rank.Ranking < minWinRank;
                        if (!score.StageClear)
                        {
                            win = false;
                        }

                        buffer.WriteByte(win ? (byte) 0 : (byte) 1); // (YouLoose=1 | YouWin=0) 
                        client.Character.IncreaseWinLoss(win, score);
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
                        client.Character.IncreaseWinLoss(win, score);
                    }

                    buffer.WriteInt32(client.Inventory.GetExpBonusPercentage());
                    buffer.WriteInt32(0); // Exp PC방 혜택
                    buffer.WriteInt32(client.Inventory.GetCoinBonusPercentage());
                    buffer.WriteInt32(0); // Coin PC방 혜택
                    buffer.WriteByte(client.Character.Level);
                    buffer.WriteInt32(client.Character.Exp);
                    buffer.WriteInt32(Character.ExpForNextLevel(client.Character));
                    if (client.BestScore != null && client.BestScore.TotalScore > totalScore)
                    {
                        buffer.WriteInt32(client.BestScore.TotalScore);
                    }
                    else
                    {
                        buffer.WriteInt32(totalScore);
                    }

                    buffer.WriteByte((byte) client.Character.GetExr(score.Mode));
                    buffer.WriteByte(0);
                }
                else
                {
                    // Watcher - Has no Score
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte((byte) client.Player.Slot);
                    buffer.WriteByte(0);
                    buffer.WriteByte(1); // Stage Clear (0=True | 1=False)
                    buffer.WriteInt16(0); // Combo
                    buffer.WriteInt16(0); // Kool
                    buffer.WriteInt16(0); // Cool
                    buffer.WriteInt16(0); // Good
                    buffer.WriteInt16(0); // Miss
                    buffer.WriteInt16(0); // Fail
                    buffer.WriteByte(0); // ComboType (0=NoBonus | 1=ALLKOOL | 2=AllCool | 3=AllCombo)
                    buffer.WriteByte(0);
                    buffer.WriteInt32(0); // Total Score
                    buffer.WriteInt32(0); // EXP Increase
                    buffer.WriteByte((byte) ScoreRankType.F); // Rank (0=F | 1=D | 2=C | 3=B | 4=A | 5=A+)
                    buffer.WriteInt16(0); // Coin Increase [Max:9999]
                    buffer.WriteInt16(client.Character.DjPoints); // MY DJ Point [Max:99]
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteInt16(client.Character.DjPointsPlus); // MY +DJ Point [Max:999]
                    buffer.WriteByte(0);
                    buffer.WriteByte(0);
                    buffer.WriteByte(0); //1 = Level Up [HP Points +1 / DJ Points +1] increase
                    buffer.WriteInt16(0); // Total Notes
                    buffer.WriteByte(client.Rank.Ranking); // Ranking (0=1st | 1=2nd | .. | 7=8th | 9=Watch)
                    buffer.WriteByte(0); // (YouLoose=1 | YouWin=0) 
                    buffer.WriteInt32(client.Inventory.GetExpBonusPercentage()); // Exp 아이템 착용 보너스 - Bonus Item
                    buffer.WriteInt32(0); // Exp PC방 혜택
                    buffer.WriteInt32(client.Inventory.GetCoinBonusPercentage()); // Coin 아이템 착용 보너스 - Bonus Item
                    buffer.WriteInt32(0); // Coin PC방 혜택
                    buffer.WriteByte(client.Character.Level); // Level [Max:99]
                    buffer.WriteInt32(client.Character.Exp); // EXP
                    buffer.WriteInt32(Character.ExpForNextLevel(client.Character)); // EXP for level up
                    if (client.BestScore != null)
                    {
                        buffer.WriteInt32(client.BestScore.TotalScore);
                    }
                    else
                    {
                        buffer.WriteInt32(0);
                    }

                    buffer.WriteByte((byte) client.Character.GetExr(client.Mode)); // EXR 
                    buffer.WriteByte(0);
                }
            }

            return buffer;
        }

        public IBuffer CreateGameOver(byte slot)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            buffer.WriteByte(slot);
            buffer.WriteByte(1);
            return buffer;
        }

        public IBuffer CreateGameStart(Song song, ModeType mode, DifficultyType difficulty)
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