using System;

namespace Arrowgene.Ez2Off.Data.Hdr
{
    public class HdrHeader
    {
        public static HdrHeader Tro()
        {
            HdrHeader header = new HdrHeader();
            header.Format = HdrFormat.Hdr;
            header.Created = null;
            header.Unknown0 = 1;
            header.IndexOffset = 20;
            header.ArchiveType = HdrArchiveType.Tro;
            return header;
        }

        public static HdrHeader Dat()
        {
            HdrHeader header = new HdrHeader();
            header.Format = HdrFormat.Hdr;
            header.Created = DateTime.Now;
            header.Unknown0 = 1;
            header.IndexOffset = 40;
            header.ArchiveType = HdrArchiveType.Dat;
            return header;
        }
        
        public HdrArchiveType ArchiveType { get; set; }
        public DateTime? Created { get; set; }
        public string Format { get; set; }
        public int Unknown0 { get; set; }
        public int ContentOffset { get; set; }
        public int FolderCount { get; set; }
        public int IndexOffset { get; set; }
    }
}