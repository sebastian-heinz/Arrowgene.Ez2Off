using System;

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class Ez2OnModelMusic
    {
        public string Name { get; set; }
        public string Duration { get; set; }
        public int Id { get; set; }
        public int Bpm { get; set; }
        public int RubyEzExr { get; set; }
        public int RubyEzNotes { get; set; }
        public int RubyShdExr { get; set; }
        public int RubyShdNotes { get; set; }
        public int ClubHdExr { get; set; }
        public int ClubHdNotes { get; set; }
        public int ClubShdNotes { get; set; }
        public SongCategoryType Category { get; set; }
        public string FileName { get; set; }
        public int LicensePrice { get; set; }
        public int New { get; set; }
        public int RubyEzActivation { get; set; }
        public int RubyEzUnknown { get; set; }
        public int RubyEzUnlock { get; set; }
        public int RubyNmExr { get; set; }
        public int RubyEzDjPoint { get; set; }
        public int RubyNmNotes { get; set; }
        public int RubyNmActivation { get; set; }
        public int RubyNmUnknown { get; set; }
        public int RubyHdExr { get; set; }
        public int RubyNmUnlock { get; set; }
        public int RubyHdNotes { get; set; }
        public int RubyNmDjPoint { get; set; }
        public int RubyHdActivation { get; set; }
        public int RubyHdUnknown { get; set; }
        public int RubyHdUnlock { get; set; }
        public int RubyHdDjPoint { get; set; }
        public int StreetEzExr { get; set; }
        public int StreetEzUnlock { get; set; }
        public int StreetEzNotes { get; set; }
        public int StreetEzDjPoint { get; set; }
        public int StreetNmActivation { get; set; }
        public int StreetNmExr { get; set; }
        public int StreetNmUnknown { get; set; }
        public int StreetNmNotes { get; set; }
        public int StreetNmUnlock { get; set; }
        public int StreetNmDjPoint { get; set; }
        public int ClubNmDjPoint { get; set; }
        public int ClubNmUnlock { get; set; }
        public int ClubNmNotes { get; set; }
        public int ClubNmUnknown { get; set; }
        public int ClubNmExr { get; set; }
        public int ClubEzDjPoint { get; set; }
        public int ClubEzUnlock { get; set; }
        public int ClubEzNotes { get; set; }
        public int ClubEzUnknown { get; set; }
        public int StreetHdExr { get; set; }
        public int StreetHdUnknown { get; set; }
        public int StreetHdNotes { get; set; }
        public int StreetHdUnlock { get; set; }
        public int StreetHdDjPoint { get; set; }
        public int StreetShdExr { get; set; }
        public int StreetShdUnknown { get; set; }
        public int StreetShdNotes { get; set; }
        public int StreetShdUnlock { get; set; }
        public int StreetShdDjPoint { get; set; }
        public int ClubEzExr { get; set; }
        public int ClubShdExr { get; set; }
        public int ClubShdUnknown { get; set; }
        public int ClubHdDjPoint { get; set; }
        public int ClubHdUnlock { get; set; }
        public int ClubHdUnknown { get; set; }
        public int ClubShdUnlock { get; set; }
        public int Unknown { get; set; }
        public int ClubShdDjPoint { get; set; }
        public int RubyShdActivation { get; set; }
        public int RubyShdUnknown { get; set; }
        public int RubyShdUnlock { get; set; }
        public int RubyShdDjPoint { get; set; }
        public int StreetEzActivation { get; set; }
        public int StreetEzUnknown { get; set; }
        public int StreetHdActivation { get; set; }
        public int StreetShdActivation { get; set; }
        public int ClubEzActivation { get; set; }
        public int ClubNmActivation { get; set; }
        public int ClubHdActivation { get; set; }
        public int ClubShdActivation { get; set; }

        public Ez2OnModelMusic()
        {
            Id = -1;
            Duration = "0:00";
            Name = "New";
            FileName = "new";
            Unknown = 1;
            RubyEzActivation = 1;
            RubyNmActivation = 2;
            RubyHdActivation = 3;
            RubyShdActivation = 4;
            StreetEzActivation = 1;
            StreetNmActivation = 2;
            StreetHdActivation = 3;
            StreetShdActivation = 4;
            ClubEzActivation = 1;
            ClubNmActivation = 2;
            ClubHdActivation = 3;
            ClubShdActivation = 4;
        }

        public int GetExr(ModeType mode, DifficultyType difficulty)
        {
            switch (mode)
            {
                case ModeType.RubyMix: return GetRubyExr(difficulty);
                case ModeType.StreetMix: return GetStreetExr(difficulty);
                case ModeType.ClubMix: return GetClubExr(difficulty);
            }

            return -1;
        }

        public int GetRubyExr(DifficultyType difficulty)
        {
            switch (difficulty)
            {
                case DifficultyType.EZ: return RubyEzExr;
                case DifficultyType.NM: return RubyNmExr;
                case DifficultyType.HD: return RubyHdExr;
                case DifficultyType.SHD: return RubyShdExr;
            }

            return -1;
        }

        public int GetStreetExr(DifficultyType difficulty)
        {
            switch (difficulty)
            {
                case DifficultyType.EZ: return StreetEzExr;
                case DifficultyType.NM: return StreetNmExr;
                case DifficultyType.HD: return StreetHdExr;
                case DifficultyType.SHD: return StreetShdExr;
            }

            return -1;
        }

        public int GetClubExr(DifficultyType difficulty)
        {
            switch (difficulty)
            {
                case DifficultyType.EZ: return ClubEzExr;
                case DifficultyType.NM: return ClubNmExr;
                case DifficultyType.HD: return ClubHdExr;
                case DifficultyType.SHD: return ClubShdExr;
            }

            return -1;
        }

        public Song ToSong()
        {
            Song song = new Song();
            song.Id = Id;
            song.Unknown = Unknown;
            song.Name = Name;
            song.Category = Category;
            song.Duration = Duration;
            song.Bpm = Bpm;
            song.FileName = FileName;
            song.New = New;
            song.LicensePrice = LicensePrice;
            song.RubyEzExr = RubyEzExr;
            song.RubyEzActivation = RubyEzActivation;
            song.RubyEzNotes = RubyEzNotes;
            song.RubyEzUnknown = RubyEzUnknown;
            song.RubyEzUnlock = RubyEzUnlock;
            song.RubyNmExr = RubyNmExr;
            song.RubyEzDjPoint = RubyEzDjPoint;
            song.RubyNmNotes = RubyNmNotes;
            song.RubyNmActivation = RubyNmActivation;
            song.RubyNmUnknown = RubyNmUnknown;
            song.RubyHdExr = RubyHdExr;
            song.RubyNmUnlock = RubyNmUnlock;
            song.RubyHdNotes = RubyHdNotes;
            song.RubyNmDjPoint = RubyNmDjPoint;
            song.RubyHdActivation = RubyHdActivation;
            song.RubyShdExr = RubyShdExr;
            song.RubyHdUnknown = RubyHdUnknown;
            song.RubyShdNotes = RubyShdNotes;
            song.RubyHdUnlock = RubyHdUnlock;
            song.RubyHdDjPoint = RubyHdDjPoint;
            song.StreetEzExr = StreetEzExr;
            song.StreetEzUnlock = StreetEzUnlock;
            song.StreetEzNotes = StreetEzNotes;
            song.StreetEzDjPoint = StreetEzDjPoint;
            song.StreetNmActivation = StreetNmActivation;
            song.StreetNmExr = StreetNmExr;
            song.StreetNmUnknown = StreetNmUnknown;
            song.StreetNmNotes = StreetNmNotes;
            song.StreetNmUnlock = StreetNmUnlock;
            song.StreetNmDjPoint = StreetNmDjPoint;
            song.StreetHdExr = StreetHdExr;
            song.StreetHdUnknown = StreetHdUnknown;
            song.StreetHdNotes = StreetHdNotes;
            song.StreetHdUnlock = StreetHdUnlock;
            song.StreetHdDjPoint = StreetHdDjPoint;
            song.StreetShdExr = StreetShdExr;
            song.StreetShdUnknown = StreetShdUnknown;
            song.StreetShdNotes = StreetShdNotes;
            song.StreetShdUnlock = StreetShdUnlock;
            song.StreetShdDjPoint = StreetShdDjPoint;
            song.ClubEzExr = ClubEzExr;
            song.ClubEzUnknown = ClubEzUnknown;
            song.ClubEzNotes = ClubEzNotes;
            song.ClubEzUnlock = ClubEzUnlock;
            song.ClubEzDjPoint = ClubEzDjPoint;
            song.ClubNmExr = ClubNmExr;
            song.ClubNmUnknown = ClubNmUnknown;
            song.ClubNmNotes = ClubNmNotes;
            song.ClubNmUnlock = ClubNmUnlock;
            song.ClubNmDjPoint = ClubNmDjPoint;
            song.ClubHdExr = ClubHdExr;
            song.ClubHdUnknown = ClubHdUnknown;
            song.ClubHdNotes = ClubHdNotes;
            song.ClubHdUnlock = ClubHdUnlock;
            song.ClubHdDjPoint = ClubHdDjPoint;
            song.ClubShdExr = ClubShdExr;
            song.ClubShdUnknown = ClubShdUnknown;
            song.ClubShdNotes = ClubShdNotes;
            song.ClubShdUnlock = ClubShdUnlock;
            song.ClubShdDjPoint = ClubShdDjPoint;
            song.RubyShdActivation = RubyShdActivation;
            song.RubyShdUnknown = RubyShdUnknown;
            song.RubyShdUnlock = RubyShdUnlock;
            song.RubyShdDjPoint = RubyShdDjPoint;
            song.StreetEzActivation = StreetEzActivation;
            song.StreetEzUnknown = StreetEzUnknown;
            song.StreetHdActivation = StreetHdActivation;
            song.StreetShdActivation = StreetShdActivation;
            song.ClubEzActivation = ClubEzActivation;
            song.ClubNmActivation = ClubNmActivation;
            song.ClubHdActivation = ClubHdActivation;
            song.ClubShdActivation = ClubShdActivation;
            return song;
        }
    }
}