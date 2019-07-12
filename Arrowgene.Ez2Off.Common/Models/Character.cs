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

using System;

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class Character
    {
        public static int ExpForNextLevel(Character character)
        {
            return character.Level * 1000;
        }

        public Character(string name)
        {
            Name = name;
            Sex = CharacterSex.Male;
            Level = 99;
            RubyExr = 0;
            StreetExr = 0;
            ClubExr = 0;
            Exp = 0;
            Coin = 9999999;
            Cash = 0;
            MaxCombo = 0;
            RubyWins = 0;
            StreetWins = 0;
            ClubWins = 0;
            RubyLoses = 0;
            StreetLoses = 0;
            ClubLoses = 0;
            Premium = 0;
        }

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
        public short Premium { get; set; }

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
    }
}