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
    public class RoomInfo
    {
        public RoomInfo()
        {
            SelectedSong = 0;
            Difficulty = DifficultyType.EZ;
            MaxDifficulty = DifficultyType.None;
            NoteEffect = NoteEffectType.None;
            FadeEffect = FadeEffectType.None;
            RandomSong = false;
            Mode = ModeType.ClubMix;
        }

        public bool AllowViewer { get; set; }
        public bool PasswordProtected { get; set; }
        public byte Number { get; set; }
        public byte MaxPlayer { get; set; }
        public int SelectedSong { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DifficultyType MaxDifficulty { get; set; }
        public DifficultyType Difficulty { get; set; }
        public GameGroupType GameGroupType { get; set; }
        public GameType GameType { get; set; }
        public bool RandomSong { get; set; }
        public NoteEffectType NoteEffect { get; set; }
        public FadeEffectType FadeEffect { get; set; }
        public ModeType Mode { get; set; }
    }
}