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
    public class RadiomixGame : Game
    {
        public RadiomixGame(Radiomix radiomix) : base()
        {
            Index = 0;
            CurrentSong = null;
            Radiomix = radiomix;
        }

        public RadiomixGame(Radiomix radiomix, RoomInfo roomInfo, Song song) : base(roomInfo, song)
        {
            Index = 0;
            CurrentSong = null;
            Radiomix = radiomix;
        }

        public void IncreaseIndex()
        {
            Index++;
        }

        public int NextSongId()
        {
            return SongId(Index + 1);
        }

        public int CurrentSongId()
        {
            return SongId(Index);
        }

        private int SongId(int index)
        {
            switch (index)
            {
                case 0: return Radiomix.Song1Id;
                case 1: return Radiomix.Song2Id;
                case 2: return Radiomix.Song3Id;
                case 3: return Radiomix.Song4Id;
            }

            return -1;
        }

        public int Index { get; private set; }
        public Song CurrentSong { get; set; }
        public Radiomix Radiomix { get; }
    }
}