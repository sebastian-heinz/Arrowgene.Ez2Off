using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Data.BinFile
{
    public class Ez2OnRadiomixBinFile : Ez2OnBinFile<Ez2OnModelRadiomix>
    {
        public override string Header => "M_RADIOMIX";

        public override Ez2OnModelRadiomix ReadEntry(IBuffer buffer)
        {
            Ez2OnModelRadiomix radioMix = new Ez2OnModelRadiomix();
            radioMix.RadiomixId = buffer.ReadInt32();
            radioMix.B = buffer.ReadInt32();
            radioMix.C = buffer.ReadInt32();
            radioMix.D = buffer.ReadInt32();
            radioMix.E = buffer.ReadInt32();
            radioMix.Song1Id = buffer.ReadInt32();
            radioMix.Song1RubyNotes = buffer.ReadInt32();
            radioMix.Song1StreetNotes = buffer.ReadInt32();
            radioMix.Song1ClubNotes = buffer.ReadInt32();
            radioMix.Song1Club8KNotes = buffer.ReadInt32();
            radioMix.Song2Id = buffer.ReadInt32();
            radioMix.Song2RubyNotes = buffer.ReadInt32();
            radioMix.Song2StreetNotes = buffer.ReadInt32();
            radioMix.Song2ClubNotes = buffer.ReadInt32();
            radioMix.Song2Club8KNotes = buffer.ReadInt32();
            radioMix.Song3Id = buffer.ReadInt32();
            radioMix.Song3RubyNotes = buffer.ReadInt32();
            radioMix.Song3StreetNotes = buffer.ReadInt32();
            radioMix.Song3ClubNotes = buffer.ReadInt32();
            radioMix.Song3Club8KNotes = buffer.ReadInt32();
            radioMix.Song4Id = buffer.ReadInt32();
            radioMix.Song4RubyNotes = buffer.ReadInt32();
            radioMix.Song4StreetNotes = buffer.ReadInt32();
            radioMix.Song4ClubNotes = buffer.ReadInt32();
            radioMix.Song4Club8KNotes = buffer.ReadInt32();
            return radioMix;
        }

        public override void WriteEntry(Ez2OnModelRadiomix radioMix, IBuffer buffer)
        {
            buffer.WriteInt32(radioMix.RadiomixId);
            buffer.WriteInt32(radioMix.B);
            buffer.WriteInt32(radioMix.C);
            buffer.WriteInt32(radioMix.D);
            buffer.WriteInt32(radioMix.E);
            buffer.WriteInt32(radioMix.Song1Id);
            buffer.WriteInt32(radioMix.Song1RubyNotes);
            buffer.WriteInt32(radioMix.Song1StreetNotes);
            buffer.WriteInt32(radioMix.Song1ClubNotes);
            buffer.WriteInt32(radioMix.Song1Club8KNotes);
            buffer.WriteInt32(radioMix.Song2Id);
            buffer.WriteInt32(radioMix.Song2RubyNotes);
            buffer.WriteInt32(radioMix.Song2StreetNotes);
            buffer.WriteInt32(radioMix.Song2ClubNotes);
            buffer.WriteInt32(radioMix.Song2Club8KNotes);
            buffer.WriteInt32(radioMix.Song3Id);
            buffer.WriteInt32(radioMix.Song3RubyNotes);
            buffer.WriteInt32(radioMix.Song3StreetNotes);
            buffer.WriteInt32(radioMix.Song3ClubNotes);
            buffer.WriteInt32(radioMix.Song3Club8KNotes);
            buffer.WriteInt32(radioMix.Song4Id);
            buffer.WriteInt32(radioMix.Song4RubyNotes);
            buffer.WriteInt32(radioMix.Song4StreetNotes);
            buffer.WriteInt32(radioMix.Song4ClubNotes);
            buffer.WriteInt32(radioMix.Song4Club8KNotes);
        }
    }
}