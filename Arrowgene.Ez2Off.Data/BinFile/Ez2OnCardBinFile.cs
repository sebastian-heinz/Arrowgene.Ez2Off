using Arrowgene.Buffers;
using Arrowgene.Ez2Off.Common.Models;

namespace Arrowgene.Ez2Off.Data.BinFile
{
    public class Ez2OnCardBinFile : Ez2OnBinFile<Ez2OnModelCard>
    {
        public override string Header => "M_CARD";

        public override Ez2OnModelCard ReadEntry(IBuffer buffer)
        {
            Ez2OnModelCard card = new Ez2OnModelCard();
            card.Id = buffer.ReadInt32();
            card.Text = ReadString(buffer);
            return card;
        }

        public override void WriteEntry(Ez2OnModelCard card, IBuffer buffer)
        {
            buffer.WriteInt32(card.Id);
            WriteString(card.Text, buffer);
        }
    }
}