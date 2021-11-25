using Arrowgene.Ez2Off.Common;

namespace Arrowgene.Ez2Off.Data.Hdr
{
    public class HdrIndex
    {
        public int Length { get; set; }
        public int Offset { get; set; }
        public int Position { get; set; }
        public byte[] NameRaw { get; set; }

        public string Name
        {
            get => Utils.KoreanEncoding.GetString(NameRaw);
            set => NameRaw = Utils.KoreanEncoding.GetBytes(value);
        }

        public HdrIndex()
        {
        }
    }
}