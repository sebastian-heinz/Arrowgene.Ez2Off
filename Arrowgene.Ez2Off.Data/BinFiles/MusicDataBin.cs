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

using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Data.BinFiles
{
    public class MusicDataBin : BinFile<Song>
    {
        public override string Header => "M_MUSIC";

        public override Song ReadEntry(IBuffer buffer)
        {
            Song song = new Song();
            song.Id = buffer.ReadInt32();
            song.Name = ReadString(buffer);
            song.b = ReadString(buffer);
            song.Duration = ReadString(buffer);
            song.Bpm = buffer.ReadInt32();
            song.e = ReadString(buffer);
            song.d1 = buffer.ReadInt32();
            song.d2 = buffer.ReadInt32();
            song.RubyEzDifficulty = buffer.ReadInt32();
            song.d4 = buffer.ReadInt32();
            song.RubyEzNotes = buffer.ReadInt32();
            song.d6 = buffer.ReadInt32();
            song.d7 = buffer.ReadInt32();
            song.d8 = buffer.ReadInt32();
            song.d9 = buffer.ReadInt32();
            song.d10 = buffer.ReadInt32();
            song.d11 = buffer.ReadInt32();
            song.d12 = buffer.ReadInt32();
            song.d13 = buffer.ReadInt32();
            song.d14 = buffer.ReadInt32();
            song.d15 = buffer.ReadInt32();
            song.d16 = buffer.ReadInt32();
            song.d17 = buffer.ReadInt32();
            song.RubyShdDifficulty = buffer.ReadInt32();
            song.d19 = buffer.ReadInt32();
            song.RubyShdNotes = buffer.ReadInt32();
            song.d21 = buffer.ReadInt32();
            song.d22 = buffer.ReadInt32();
            song.d23 = buffer.ReadInt32();
            song.d24 = buffer.ReadInt32();
            song.d25 = buffer.ReadInt32();
            song.d26 = buffer.ReadInt32();
            song.d27 = buffer.ReadInt32();
            song.d28 = buffer.ReadInt32();
            song.d29 = buffer.ReadInt32();
            song.d30 = buffer.ReadInt32();
            song.d31 = buffer.ReadInt32();
            song.d32 = buffer.ReadInt32();
            song.d33 = buffer.ReadInt32();
            song.d34 = buffer.ReadInt32();
            song.d35 = buffer.ReadInt32();
            song.d36 = buffer.ReadInt32();
            song.d37 = buffer.ReadInt32();
            song.d38 = buffer.ReadInt32();
            song.d39 = buffer.ReadInt32();
            song.d40 = buffer.ReadInt32();
            song.d41 = buffer.ReadInt32();
            song.d42 = buffer.ReadInt32();
            song.d43 = buffer.ReadInt32();
            song.d44 = buffer.ReadInt32();
            song.d45 = buffer.ReadInt32();
            song.d46 = buffer.ReadInt32();
            song.d47 = buffer.ReadInt32();
            song.d48 = buffer.ReadInt32();
            song.d49 = buffer.ReadInt32();
            song.d50 = buffer.ReadInt32();
            song.d51 = buffer.ReadInt32();
            song.d52 = buffer.ReadInt32();
            song.ClubHdDifficulty = buffer.ReadInt32();
            song.d54 = buffer.ReadInt32();
            song.ClubHdNotes = buffer.ReadInt32();
            song.d56 = buffer.ReadInt32();
            song.d57 = buffer.ReadInt32();
            song.d58 = buffer.ReadInt32();
            song.d59 = buffer.ReadInt32();
            song.ClubShdNotes = buffer.ReadInt32();
            song.d61 = buffer.ReadInt32();
            return song;
        }

        public override void WriteEntry(Song song, IBuffer buffer)
        {
            buffer.WriteInt32(song.Id);
            WriteString(song.Name, buffer);
            WriteString(song.b, buffer);
            WriteString(song.Duration, buffer);
            buffer.WriteInt32(song.Bpm);
            WriteString(song.e, buffer);
            buffer.WriteInt32(song.d1);
            buffer.WriteInt32(song.d2);
            buffer.WriteInt32(song.RubyEzDifficulty);
            buffer.WriteInt32(song.d4);
            buffer.WriteInt32(song.RubyEzNotes);
            buffer.WriteInt32(song.d6);
            buffer.WriteInt32(song.d7);
            buffer.WriteInt32(song.d8);
            buffer.WriteInt32(song.d9);
            buffer.WriteInt32(song.d10);
            buffer.WriteInt32(song.d11);
            buffer.WriteInt32(song.d12);
            buffer.WriteInt32(song.d13);
            buffer.WriteInt32(song.d14);
            buffer.WriteInt32(song.d15);
            buffer.WriteInt32(song.d16);
            buffer.WriteInt32(song.d17);
            buffer.WriteInt32(song.RubyShdDifficulty);
            buffer.WriteInt32(song.d19);
            buffer.WriteInt32(song.RubyShdNotes);
            buffer.WriteInt32(song.d21);
            buffer.WriteInt32(song.d22);
            buffer.WriteInt32(song.d23);
            buffer.WriteInt32(song.d24);
            buffer.WriteInt32(song.d25);
            buffer.WriteInt32(song.d26);
            buffer.WriteInt32(song.d27);
            buffer.WriteInt32(song.d28);
            buffer.WriteInt32(song.d29);
            buffer.WriteInt32(song.d30);
            buffer.WriteInt32(song.d31);
            buffer.WriteInt32(song.d32);
            buffer.WriteInt32(song.d33);
            buffer.WriteInt32(song.d34);
            buffer.WriteInt32(song.d35);
            buffer.WriteInt32(song.d36);
            buffer.WriteInt32(song.d37);
            buffer.WriteInt32(song.d38);
            buffer.WriteInt32(song.d39);
            buffer.WriteInt32(song.d40);
            buffer.WriteInt32(song.d41);
            buffer.WriteInt32(song.d42);
            buffer.WriteInt32(song.d43);
            buffer.WriteInt32(song.d44);
            buffer.WriteInt32(song.d45);
            buffer.WriteInt32(song.d46);
            buffer.WriteInt32(song.d47);
            buffer.WriteInt32(song.d48);
            buffer.WriteInt32(song.d49);
            buffer.WriteInt32(song.d50);
            buffer.WriteInt32(song.d51);
            buffer.WriteInt32(song.d52);
            buffer.WriteInt32(song.ClubHdDifficulty);
            buffer.WriteInt32(song.d54);
            buffer.WriteInt32(song.ClubHdNotes);
            buffer.WriteInt32(song.d56);
            buffer.WriteInt32(song.d57);
            buffer.WriteInt32(song.d58);
            buffer.WriteInt32(song.d59);
            buffer.WriteInt32(song.ClubShdNotes);
            buffer.WriteInt32(song.d61);
        }
    }
}