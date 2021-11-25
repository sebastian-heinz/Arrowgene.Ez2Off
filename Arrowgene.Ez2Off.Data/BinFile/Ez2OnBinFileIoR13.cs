using Arrowgene.Buffers;
using Arrowgene.Ez2Off.Common;

namespace Arrowgene.Ez2Off.Data.BinFile
{
    public class Ez2OnBinFileIoR13
    {
        public Ez2OnBinFile Read(string source)
        {
            byte[] dataBin = Utils.ReadFile(source);
            if (dataBin.Length < Ez2OnBinFile.HeaderSize)
            {
                return null;
            }
            IBuffer buffer = new StreamBuffer(dataBin);
            buffer.SetPositionStart();
            string header = buffer.ReadCString();
            Ez2OnBinFile file;
            switch (header)
            {
                case "M_CARD":
                    file = new Ez2OnCardBinFile();
                    break;
                case "M_ID_FILTER":
                    file = new Ez2OnIdFilterBinFile();
                    break;
                case "M_ITEM":
                    file = new Ez2OnItemBinFileR13();
                    break;
                case "M_MUSIC":
                    file = new Ez2OnMusicBinFileR13();
                    break;
                case "M_QUEST":
                    file = new Ez2OnQuestBinFile();
                    break;
                default: return null;
            }
            file.Read(buffer);
            return file;
        }

        public void Write(string destination, Ez2OnBinFile binFile)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.SetPositionStart();
            binFile.Write(buffer);
            Utils.WriteFile(buffer.GetAllBytes(), destination);
        }

    }
}
