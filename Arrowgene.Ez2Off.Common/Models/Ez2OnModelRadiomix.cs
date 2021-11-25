using System;

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class Ez2OnModelRadiomix
    {
        public int RadiomixId { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public int Song1Id { get; set; }
        public int Song1RubyNotes { get; set; }
        public int Song1StreetNotes { get; set; }
        public int Song1ClubNotes { get; set; }
        public int Song1Club8KNotes { get; set; }
        public int Song2Id { get; set; }
        public int Song2RubyNotes { get; set; }
        public int Song2StreetNotes { get; set; }
        public int Song2ClubNotes { get; set; }
        public int Song2Club8KNotes { get; set; }
        public int Song3Id { get; set; }
        public int Song3RubyNotes { get; set; }
        public int Song3StreetNotes { get; set; }
        public int Song3ClubNotes { get; set; }
        public int Song3Club8KNotes { get; set; }
        public int Song4Id { get; set; }
        public int Song4RubyNotes { get; set; }
        public int Song4StreetNotes { get; set; }
        public int Song4ClubNotes { get; set; }
        public int Song4Club8KNotes { get; set; }

        public int TotalRubyNotes => Song1RubyNotes + Song2RubyNotes + Song3RubyNotes + Song4RubyNotes;
        public int TotalStreetNotes => Song1StreetNotes + Song2StreetNotes + Song3StreetNotes + Song4StreetNotes;
        public int TotalClubNotes => Song1ClubNotes + Song2ClubNotes + Song3ClubNotes + Song4ClubNotes;
        public int TotalCLub8KNotes => Song1Club8KNotes + Song2Club8KNotes + Song3Club8KNotes + Song4Club8KNotes;

        public Radiomix ToRadiomix()
        {
            Radiomix radiomix = new Radiomix();
            radiomix.Id = RadiomixId;
            radiomix.B = B;
            radiomix.C = C;
            radiomix.D = D;
            radiomix.E = E;
            radiomix.Song1Id = Song1Id;
            radiomix.Song1RubyNotes = Song1RubyNotes;
            radiomix.Song1StreetNotes = Song1StreetNotes;
            radiomix.Song1ClubNotes = Song1ClubNotes;
            radiomix.Song1Club8KNotes = Song1Club8KNotes;
            radiomix.Song2Id = Song2Id;
            radiomix.Song2RubyNotes = Song2RubyNotes;
            radiomix.Song2StreetNotes = Song2StreetNotes;
            radiomix.Song2ClubNotes = Song2ClubNotes;
            radiomix.Song2Club8KNotes = Song2Club8KNotes;
            radiomix.Song3Id = Song3Id;
            radiomix.Song3RubyNotes = Song3RubyNotes;
            radiomix.Song3StreetNotes = Song3StreetNotes;
            radiomix.Song3ClubNotes = Song3ClubNotes;
            radiomix.Song3Club8KNotes = Song3Club8KNotes;
            radiomix.Song4Id = Song4Id;
            radiomix.Song4RubyNotes = Song4RubyNotes;
            radiomix.Song4StreetNotes = Song4StreetNotes;
            radiomix.Song4ClubNotes = Song4ClubNotes;
            radiomix.Song4Club8KNotes = Song4Club8KNotes;
            return radiomix;
        }
    }
}