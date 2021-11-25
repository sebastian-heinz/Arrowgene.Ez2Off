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
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Data.BinFile
{
    public class Ez2OnMusicBinFileR13 : Ez2OnBinFile<Ez2OnModelMusic>
    {
        public override string Header => "M_MUSIC";

        public override Ez2OnModelMusic ReadEntry(IBuffer buffer)
        {
            Ez2OnModelMusic song = new Ez2OnModelMusic();
            song.Id = buffer.ReadInt32();
            song.Name = ReadString(buffer);
            song.Category = GetSongCategory(ReadString(buffer));
            song.Duration = ReadString(buffer);
            song.Bpm = buffer.ReadInt32();
            song.FileName = ReadString(buffer);
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.RubyEzExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.RubyEzNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.RubyNmExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.RubyNmNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.RubyHdExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.RubyHdNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.RubyShdExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.RubyShdNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.StreetEzExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.StreetEzNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.StreetNmExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.StreetNmNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.StreetHdExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.StreetHdNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.StreetShdExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.StreetShdNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.ClubEzExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.ClubEzNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.ClubNmExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.ClubNmNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.ClubHdExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.ClubHdNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            buffer.ReadInt32();
            song.ClubShdExr = buffer.ReadInt32();
            buffer.ReadInt32();
            song.ClubShdNotes = buffer.ReadInt32();
            buffer.ReadInt32();

            return song;
        }

        public override void WriteEntry(Ez2OnModelMusic song, IBuffer buffer)
        {
            buffer.WriteInt32(song.Id);
            WriteString(song.Name, buffer);
            WriteString(GetSongCategory(song.Category), buffer);
            WriteString(song.Duration, buffer);
            buffer.WriteInt32(song.Bpm);
            WriteString(song.FileName, buffer);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.RubyEzExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.RubyEzNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.RubyNmExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.RubyNmNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.RubyHdExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.RubyHdNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.RubyShdExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.RubyShdNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.StreetEzExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.StreetEzNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.StreetNmExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.StreetNmNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.StreetHdExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.ClubHdNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.StreetShdExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.StreetShdNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.ClubEzExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.ClubEzNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.ClubNmExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.ClubNmNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.ClubHdExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.ClubHdNotes);
            buffer.WriteInt32(0);

            buffer.WriteInt32(0);
            buffer.WriteInt32(song.ClubShdExr);
            buffer.WriteInt32(0);
            buffer.WriteInt32(song.ClubShdNotes);
            buffer.WriteInt32(0);
        }

        private SongCategoryType GetSongCategory(string category)
        {
            if (!int.TryParse(category, out int categoryNum))
            {
                return SongCategoryType.None;
            }

            if (!Enum.IsDefined(typeof(SongCategoryType), categoryNum))
            {
                return SongCategoryType.None;
            }

            return (SongCategoryType) categoryNum;
        }

        private string GetSongCategory(SongCategoryType category)
        {
            return ((int) category).ToString();
        }
    }
}