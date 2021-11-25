namespace Arrowgene.Ez2Off.Common.Models
{
    public class Radiomix
    {
        public int Id { get; set; }
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
    }
}