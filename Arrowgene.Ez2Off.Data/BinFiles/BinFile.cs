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
using System.Collections.Generic;
using System.Text;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Data.BinFiles
{
    public abstract class BinFile<T>
    {
        public const int HeaderSize = 12;

        public BinFile()
        {
            Entries = new List<T>();
        }

        public abstract string Header { get; }
        public List<T> Entries { get; }

        public abstract T ReadEntry(IBuffer buffer);

        public abstract void WriteEntry(T entry, IBuffer buffer);

        public void Read(string path)
        {
            byte[] dataBin = Utils.ReadFile(path);
            if (dataBin.Length < HeaderSize)
            {
                throw new Exception("Invalid file size.");
            }

            IBuffer buffer = new StreamBuffer(dataBin);
            buffer.SetPositionStart();
            string header = buffer.ReadString(Header.Length);
            if (header != Header)
            {
                throw new Exception("Invalid header.");
            }

            buffer.Position = HeaderSize;
            int itemCount = buffer.ReadInt32();
            for (int i = 0; i < itemCount; i++)
            {
                T entry = ReadEntry(buffer);
                Entries.Add(entry);
            }
        }

        public void Write(string path)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteString(Header);
            buffer.Position = HeaderSize;
            buffer.WriteInt32(Entries.Count);
            for (int i = 1; i < Entries.Count; i++)
            {
                T entry = Entries[i];
                WriteEntry(entry, buffer);
            }

            Utils.WriteFile(buffer.GetAllBytes(), path);
        }

        protected string ReadString(IBuffer buffer)
        {
            int count = buffer.ReadInt32();
            string base64 = buffer.ReadString(count);
            byte[] base64Bytes = Convert.FromBase64String(base64);
            string decodedString = Utils.KoreanEncoding.GetString(base64Bytes);
            return decodedString;
        }

        protected void WriteString(string decodedString, IBuffer buffer)
        {
            byte[] base64Bytes = Utils.KoreanEncoding.GetBytes(decodedString);
            string base64 = Convert.ToBase64String(base64Bytes);
            buffer.WriteInt32(base64.Length);
            buffer.WriteString(base64);
        }
    }
}