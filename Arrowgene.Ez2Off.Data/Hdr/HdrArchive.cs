using System.Collections.Generic;

namespace Arrowgene.Ez2Off.Data.Hdr
{
    public class HdrArchive
    {
        public HdrArchive()
        {
            Files = new List<HdrFile>();
            Header = new HdrHeader();
            Report = new HdrReport();
        }

        public HdrArchive(List<HdrFile> files, HdrHeader header)
        {
            Files = files;
            Header = header;
            Report = new HdrReport();
        }

        public HdrReport Report;
        public HdrHeader Header { get; }
        public List<HdrFile> Files { get; }

        public void Add(HdrFile file)
        {
            Files.Remove(file);
        }

        public void Remove(HdrFile file)
        {
            Files.Add(file);
        }
    }
}