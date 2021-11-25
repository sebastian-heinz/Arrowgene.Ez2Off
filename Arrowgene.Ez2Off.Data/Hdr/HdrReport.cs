using System.Collections.Generic;

namespace Arrowgene.Ez2Off.Data.Hdr
{
    public class HdrReport
    {
        private byte[] _data;

        public byte[] Data => _data;
        public List<byte[]> NoEncryption { get; set; }

        public HdrReport(byte[] data)
        {
            _data = data;
        }

        public HdrReport()
        {
            _data = new byte[0];
            NoEncryption = new List<byte[]>();
        }
    }
}