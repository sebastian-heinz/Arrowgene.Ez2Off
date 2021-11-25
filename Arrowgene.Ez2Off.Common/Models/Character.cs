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

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class Character
    {
        public const int MaxLevel = 99;

        public static int ExpForLevel(int level)
        {
            double baseValue = 505 * level;
            return (int) baseValue;
        }

        public static int ExpForNextLevel(Character character)
        {
            return ExpForLevel(character.Level);
        }

        public static int CalculateExpGain(Score score)
        {
            int baseExp = score.TotalScore;
            double expEarned = baseExp / (double) 10000;
            if (score.Game.Type == GameType.SinglePlayer)
            {
                expEarned *= 6;
            }
            else if (score.Game.Type == GameType.MultiPLayer)
            {
                expEarned *= 9;
            }

            int expResult = (int) Math.Ceiling(expEarned);
            return expResult;
        }

        public static int CalculateItemExpGain(int exp, int bonusPercent)
        {
            return (int) (exp * (bonusPercent / 100d));
        }

        public static int CalculateCoinGain(Score score)
        {
            int baseCoin = score.TotalScore;
            double coinEarned = baseCoin / (double) 10000;
            if (score.Game.Type == GameType.SinglePlayer)
            {
                coinEarned *= 3; //1.1
            }
            else if (score.Game.Type == GameType.MultiPLayer)
            {
                coinEarned *= 5; //2
            }

            int coinResult = (int) Math.Ceiling(coinEarned);
            return coinResult;
        }

        public static int CalculateItemCoinGain(int coin, int bonusPercent)
        {
            return (int) (coin * (bonusPercent / 100d));
        }

        public static int CalculateItemHpGain(int hp, int bonusPercent)
        {
            return (int) (hp * (bonusPercent / 100d));
        }

        public Character()
        {
            Id = -1;
            Name = "";
            Sex = CharacterSex.Male;
            Level = 1;
            RubyExr = 0;
            StreetExr = 0;
            ClubExr = 0;
            Exp = 0;
            Coin = 100000;
            Cash = 10000;
            MaxCombo = 0;
            RubyWins = 0;
            StreetWins = 0;
            ClubWins = 0;
            RubyLoses = 0;
            StreetLoses = 0;
            ClubLoses = 0;
            Premium = 0;
            DjPoints = 77;
            DjPointsPlus = 0;
            ClubMaxScore = 0;
            StreetMaxScore = 0;
            RubyMaxScore = 0;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public CharacterSex Sex { get; set; }
        public byte Level { get; set; }
        public int RubyExr { get; set; }
        public int StreetExr { get; set; }
        public int ClubExr { get; set; }
        public int Exp { get; set; }
        public int Coin { get; set; }
        public int Cash { get; set; }
        public short MaxCombo { get; set; }
        public int RubyWins { get; set; }
        public int StreetWins { get; set; }
        public int ClubWins { get; set; }
        public int RubyLoses { get; set; }
        public int StreetLoses { get; set; }
        public int ClubLoses { get; set; }
        public int RubyMaxScore { get; set; }
        public int StreetMaxScore { get; set; }
        public int ClubMaxScore { get; set; }
        public short Premium { get; set; }
        public short DjPoints { get; set; }
        public short DjPointsPlus { get; set; }

        public void Load(Character character)
        {
            Id = character.Id;
            Name = character.Name;
            Sex = character.Sex;
            Level = character.Level;
            RubyExr = character.RubyExr;
            StreetExr = character.StreetExr;
            ClubExr = character.ClubExr;
            Exp = character.Exp;
            Coin = character.Coin;
            Cash = character.Cash;
            MaxCombo = character.MaxCombo;
            RubyWins = character.RubyWins;
            StreetWins = character.StreetWins;
            ClubWins = character.ClubWins;
            RubyLoses = character.RubyLoses;
            StreetLoses = character.StreetLoses;
            ClubLoses = character.ClubLoses;
            Premium = character.Premium;
            DjPoints = character.DjPoints;
            DjPointsPlus = character.DjPointsPlus;
            RubyMaxScore = character.RubyMaxScore;
            StreetMaxScore = character.StreetMaxScore;
            ClubMaxScore = character.ClubMaxScore;
        }

        public int GetHp()
        {
            return 99 + Level;
        }

        public void IncreaseMaxCombo(Score score)
        {
            if (score.MaxCombo > MaxCombo)
            {
                MaxCombo = (short) score.MaxCombo;
            }
        }

        public void IncreaseWinLoss(bool win, Score score)
        {
            if (score.Game.Type != GameType.MultiPLayer)
            {
                return;
            }

            if (win)
            {
                switch (score.Mode)
                {
                    case ModeType.ClubMix:
                        ClubWins++;
                        break;
                    case ModeType.StreetMix:
                        StreetWins++;
                        break;
                    case ModeType.RubyMix:
                        RubyWins++;
                        break;
                }
            }
            else
            {
                switch (score.Mode)
                {
                    case ModeType.ClubMix:
                        ClubLoses++;
                        break;
                    case ModeType.StreetMix:
                        StreetLoses++;
                        break;
                    case ModeType.RubyMix:
                        RubyLoses++;
                        break;
                }
            }
        }

        public void AddExp(int exp)
        {
            if (Level >= MaxLevel)
            {
                Exp = 0;
                return;
            }

            Exp += exp;
        }

        public void AddCoin(int coin)
        {
            Coin += coin;
        }

        public void AddCash(int cash)
        {
            Cash += cash;
        }

        public bool IsLevelUp()
        {
            if (Level == MaxLevel)
            {
                return false;
            }

            int nextLevelExp = ExpForNextLevel(this);
            if (Exp >= nextLevelExp)
            {
                Exp = 0;
                Level++;
                return true;
            }

            return false;
        }

        public int GetExr(ModeType modeType)
        {
            switch (modeType)
            {
                case ModeType.ClubMix: return ClubExr;
                case ModeType.StreetMix: return StreetExr;
                case ModeType.RubyMix: return RubyExr;
                default: return 0;
            }
        }

        /// <summary>
        /// Increase EXR for score.
        /// </summary>
        public void IncreaseExr(Score score)
        {
            if (score.ComboType == ComboType.AllCombo 
            || score.ComboType == ComboType.AllCool 
            || score.ComboType == ComboType.AllKool 
            /*
            || score.Kool + score.Cool + score.Good + score.Miss == score.TotalNotes*/
            )
            {
                IncreaseExr(score.Mode, score.Song, score.Difficulty);
            }
        }

        /// <summary>
        /// Increase EXR if eligible.
        /// </summary>
        private void IncreaseExr(ModeType mode, Song song, DifficultyType difficultyType)
        {
            if (song == null)
            {
                return;
            }

            switch (mode)
            {
                case ModeType.RubyMix:
                {
                    int exr = song.GetRubyExr(difficultyType);
                    if (exr > RubyExr)
                    {
                        RubyExr = exr;
                    }

                    break;
                }
                case ModeType.StreetMix:
                {
                    int exr = song.GetStreetExr(difficultyType);
                    if (exr > StreetExr)
                    {
                        StreetExr = exr;
                    }

                    break;
                }
                case ModeType.ClubMix:
                {
                    int exr = song.GetClubExr(difficultyType);
                    if (exr > ClubExr)
                    {
                        ClubExr = exr;
                    }

                    break;
                }
            }
        }
    }
}