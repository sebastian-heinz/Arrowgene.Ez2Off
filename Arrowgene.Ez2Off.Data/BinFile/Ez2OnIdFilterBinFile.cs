using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Data.BinFile
{
    public class Ez2OnIdFilterBinFile : Ez2OnBinFile<string>
    {
        public override string Header => "M_ID_FILTER";

        public override string ReadEntry(IBuffer buffer)
        {
            return ReadString(buffer);
        }

        public override void WriteEntry(string idFilter, IBuffer buffer)
        {
            WriteString(idFilter, buffer);
        }
    }
}