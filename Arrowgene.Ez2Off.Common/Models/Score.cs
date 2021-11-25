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
    public class Score
    {
        public static ComboType GetComboType(Score score)
        {
            if (score.MaxCombo == score.TotalNotes)
            {
                if (score.Good == 0 && score.Fail == 0) //&& score.Miss == 0
                {
                    if (score.Cool == 0)
                    {
                        return ComboType.AllKool;
                    }

                    return ComboType.AllCool;
                }

                return ComboType.AllCombo;
            }

            return ComboType.None;
        }

        public const int AllComboBonus = 10000;
        public const int AllCoolBonus = 30000;
        public const int AllKoolBonus = 50000;

        public Score()
        {
            Id = -1;
        }

        public int Id { get; set; }
        public Game Game { get; set; }
        public Character Character { get; set; }
        public Song Song { get; set; }
        public DifficultyType Difficulty { get; set; }
        public bool StageClear { get; set; }
        public int MaxCombo { get; set; }
        public int Kool { get; set; }
        public int Cool { get; set; }
        public int Good { get; set; }
        public int Miss { get; set; }
        public int Fail { get; set; }
        public int RawScore { get; set; }
        public ScoreRankType Rank { get; set; }
        public int TotalNotes { get; set; }
        public ComboType ComboType { get; set; }
        public int TotalScore => CalulateTotalScore();
        public NoteEffectType NoteEffect { get; set; }
        public FadeEffectType FadeEffect { get; set; }
        public DateTime Created { get; set; }
        public ModeType Mode { get; set; }
        public bool Incident { get; set; }

        private int CalulateTotalScore()
        {
            switch (ComboType)
            {
                case ComboType.AllKool: return RawScore + AllKoolBonus;
                case ComboType.AllCool: return RawScore + AllCoolBonus;
                case ComboType.AllCombo: return RawScore + AllComboBonus;
                default: return RawScore;
            }
        }
    }
}