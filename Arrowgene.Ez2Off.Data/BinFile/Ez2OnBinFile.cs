using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Data.BinFile
{
    public abstract class Ez2OnBinFile
    {
        public const int HeaderSize = 12;
        // 949 | ks_c_5601-1987 | Korean     
        public static readonly Encoding KoreanEncoding = CodePagesEncodingProvider.Instance.GetEncoding(949);
        public abstract string Header { get; }
        public abstract void Read(IBuffer buffer);
        public abstract void Write(IBuffer buffer);
        public abstract object GetEntry(int index);
        public abstract void SetEntry(int index, object entry);
    }

    public abstract class Ez2OnBinFile<T> : Ez2OnBinFile
    {
        public Ez2OnBinFile()
        {
            Entries = new List<T>();
        }

        public List<T> Entries { get; }

        public abstract T ReadEntry(IBuffer buffer);

        public abstract void WriteEntry(T entry, IBuffer buffer);

        public override void Read(IBuffer buffer)
        {
            if (buffer.Size < Ez2OnBinFile.HeaderSize)
            {
                throw new Exception("Invalid file size.");
            }
            buffer.SetPositionStart();
            string header = buffer.ReadString(Header.Length);
            if (header != Header)
            {
                throw new Exception("Invalid header.");
            }

            buffer.Position = Ez2OnBinFile.HeaderSize;
            int itemCount = buffer.ReadInt32();
            for (int i = 0; i < itemCount; i++)
            {
                T entry = ReadEntry(buffer);
                Entries.Add(entry);
            }
        }

        public override void Write(IBuffer buffer)
        {
            buffer.WriteString(Header);
            buffer.Position = Ez2OnBinFile.HeaderSize;
            buffer.WriteInt32(Entries.Count);
            for (int i = 0; i < Entries.Count; i++)
            {
                T entry = Entries[i];
                WriteEntry(entry, buffer);
            }
        }

        public override object GetEntry(int index)
        {
            return Entries[index];
        }

        public override void SetEntry(int index, object entry)
        {
            Entries[index] = (T)entry;
        }

        protected string ReadString(IBuffer buffer)
        {
            int count = buffer.ReadInt32();
            string base64 = buffer.ReadString(count);
            byte[] base64Bytes = Convert.FromBase64String(base64);
            string decodedString = Ez2OnBinFile.KoreanEncoding.GetString(base64Bytes);
            return decodedString;
        }

        protected void WriteString(string decodedString, IBuffer buffer)
        {
            byte[] base64Bytes = Ez2OnBinFile.KoreanEncoding.GetBytes(decodedString);
            string base64 = Convert.ToBase64String(base64Bytes);
            buffer.WriteInt32(base64.Length);
            buffer.WriteString(base64);
        }
    }
}