using System;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Data.BinFile
{
    public class Ez2OnMusicBinFile : Ez2OnBinFile<Ez2OnModelMusic>
    {
        public override string Header => "M_MUSIC";

        public override Ez2OnModelMusic ReadEntry(IBuffer buffer)
        {
            Ez2OnModelMusic song = new Ez2OnModelMusic();
            song.Id = buffer.ReadInt32();
            song.Unknown = buffer.ReadInt32();
            song.Name = ReadString(buffer);
            song.Category = GetSongCategory(ReadString(buffer));
            song.Duration = ReadString(buffer);
            song.Bpm = buffer.ReadInt32();
            song.FileName = ReadString(buffer);
            song.New = buffer.ReadInt32();
            song.LicensePrice = buffer.ReadInt32();

            song.RubyEzActivation = buffer.ReadInt32();
            song.RubyEzExr = buffer.ReadInt32();
            song.RubyEzUnknown = buffer.ReadInt32();
            song.RubyEzNotes = buffer.ReadInt32();
            song.RubyEzUnlock = buffer.ReadInt32();
            song.RubyEzDjPoint = buffer.ReadInt32();

            song.RubyNmActivation = buffer.ReadInt32();
            song.RubyNmExr = buffer.ReadInt32();
            song.RubyNmUnknown = buffer.ReadInt32();
            song.RubyNmNotes = buffer.ReadInt32();
            song.RubyNmUnlock = buffer.ReadInt32();
            song.RubyNmDjPoint = buffer.ReadInt32();

            song.RubyHdActivation = buffer.ReadInt32();
            song.RubyHdExr = buffer.ReadInt32();
            song.RubyHdUnknown = buffer.ReadInt32();
            song.RubyHdNotes = buffer.ReadInt32();
            song.RubyHdUnlock = buffer.ReadInt32();
            song.RubyHdDjPoint = buffer.ReadInt32();

            song.RubyShdActivation = buffer.ReadInt32();
            song.RubyShdExr = buffer.ReadInt32();
            song.RubyShdUnknown = buffer.ReadInt32();
            song.RubyShdNotes = buffer.ReadInt32();
            song.RubyShdUnlock = buffer.ReadInt32();
            song.RubyShdDjPoint = buffer.ReadInt32();

            song.StreetEzActivation = buffer.ReadInt32();
            song.StreetEzExr = buffer.ReadInt32();
            song.StreetEzUnknown = buffer.ReadInt32();
            song.StreetEzNotes = buffer.ReadInt32();
            song.StreetEzUnlock = buffer.ReadInt32();
            song.StreetEzDjPoint = buffer.ReadInt32();

            song.StreetNmActivation = buffer.ReadInt32();
            song.StreetNmExr = buffer.ReadInt32();
            song.StreetNmUnknown = buffer.ReadInt32();
            song.StreetNmNotes = buffer.ReadInt32();
            song.StreetNmUnlock = buffer.ReadInt32();
            song.StreetNmDjPoint = buffer.ReadInt32();

            song.StreetHdActivation = buffer.ReadInt32();
            song.StreetHdExr = buffer.ReadInt32();
            song.StreetHdUnknown = buffer.ReadInt32();
            song.StreetHdNotes = buffer.ReadInt32();
            song.StreetHdUnlock = buffer.ReadInt32();
            song.StreetHdDjPoint = buffer.ReadInt32();

            song.StreetShdActivation = buffer.ReadInt32();
            song.StreetShdExr = buffer.ReadInt32();
            song.StreetShdUnknown = buffer.ReadInt32();
            song.StreetShdNotes = buffer.ReadInt32();
            song.StreetShdUnlock = buffer.ReadInt32();
            song.StreetShdDjPoint = buffer.ReadInt32();

            song.ClubEzActivation = buffer.ReadInt32();
            song.ClubEzExr = buffer.ReadInt32();
            song.ClubEzUnknown = buffer.ReadInt32();
            song.ClubEzNotes = buffer.ReadInt32();
            song.ClubEzUnlock = buffer.ReadInt32();
            song.ClubEzDjPoint = buffer.ReadInt32();

            song.ClubNmActivation = buffer.ReadInt32();
            song.ClubNmExr = buffer.ReadInt32();
            song.ClubNmUnknown = buffer.ReadInt32();
            song.ClubNmNotes = buffer.ReadInt32();
            song.ClubNmUnlock = buffer.ReadInt32();
            song.ClubNmDjPoint = buffer.ReadInt32();

            song.ClubHdActivation = buffer.ReadInt32();
            song.ClubHdExr = buffer.ReadInt32();
            song.ClubHdUnknown = buffer.ReadInt32();
            song.ClubHdNotes = buffer.ReadInt32();
            song.ClubHdUnlock = buffer.ReadInt32();
            song.ClubHdDjPoint = buffer.ReadInt32();

            song.ClubShdActivation = buffer.ReadInt32();
            song.ClubShdExr = buffer.ReadInt32();
            song.ClubShdUnknown = buffer.ReadInt32();
            song.ClubShdNotes = buffer.ReadInt32();
            song.ClubShdUnlock = buffer.ReadInt32();
            song.ClubShdDjPoint = buffer.ReadInt32();

            return song;
        }

        public override void WriteEntry(Ez2OnModelMusic song, IBuffer buffer)
        {
            buffer.WriteInt32(song.Id);
            buffer.WriteInt32(song.Unknown);
            WriteString(song.Name, buffer);
            WriteString(GetSongCategory(song.Category), buffer);
            WriteString(song.Duration, buffer);
            buffer.WriteInt32(song.Bpm);
            WriteString(song.FileName, buffer);
            buffer.WriteInt32(song.New);
            buffer.WriteInt32(song.LicensePrice);

            buffer.WriteInt32(song.RubyEzActivation);
            buffer.WriteInt32(song.RubyEzExr);
            buffer.WriteInt32(song.RubyEzUnknown);
            buffer.WriteInt32(song.RubyEzNotes);
            buffer.WriteInt32(song.RubyEzUnlock);
            buffer.WriteInt32(song.RubyEzDjPoint);

            buffer.WriteInt32(song.RubyNmActivation);
            buffer.WriteInt32(song.RubyNmExr);
            buffer.WriteInt32(song.RubyNmUnknown);
            buffer.WriteInt32(song.RubyNmNotes);
            buffer.WriteInt32(song.RubyNmUnlock);
            buffer.WriteInt32(song.RubyNmDjPoint);

            buffer.WriteInt32(song.RubyHdActivation);
            buffer.WriteInt32(song.RubyHdExr);
            buffer.WriteInt32(song.RubyHdUnknown);
            buffer.WriteInt32(song.RubyHdNotes);
            buffer.WriteInt32(song.RubyHdUnlock);
            buffer.WriteInt32(song.RubyHdDjPoint);

            buffer.WriteInt32(song.RubyShdActivation);
            buffer.WriteInt32(song.RubyShdExr);
            buffer.WriteInt32(song.RubyShdUnknown);
            buffer.WriteInt32(song.RubyShdNotes);
            buffer.WriteInt32(song.RubyShdUnlock);
            buffer.WriteInt32(song.RubyShdDjPoint);

            buffer.WriteInt32(song.StreetEzActivation);
            buffer.WriteInt32(song.StreetEzExr);
            buffer.WriteInt32(song.StreetEzUnknown);
            buffer.WriteInt32(song.StreetEzNotes);
            buffer.WriteInt32(song.StreetEzUnlock);
            buffer.WriteInt32(song.StreetEzDjPoint);

            buffer.WriteInt32(song.StreetNmActivation);
            buffer.WriteInt32(song.StreetNmExr);
            buffer.WriteInt32(song.StreetNmUnknown);
            buffer.WriteInt32(song.StreetNmNotes);
            buffer.WriteInt32(song.StreetNmUnlock);
            buffer.WriteInt32(song.StreetNmDjPoint);

            buffer.WriteInt32(song.StreetHdActivation);
            buffer.WriteInt32(song.StreetHdExr);
            buffer.WriteInt32(song.StreetHdUnknown);
            buffer.WriteInt32(song.ClubHdNotes);
            buffer.WriteInt32(song.StreetHdUnlock);
            buffer.WriteInt32(song.StreetHdDjPoint);

            buffer.WriteInt32(song.StreetShdActivation);
            buffer.WriteInt32(song.StreetShdExr);
            buffer.WriteInt32(song.StreetShdUnknown);
            buffer.WriteInt32(song.StreetShdNotes);
            buffer.WriteInt32(song.StreetShdUnlock);
            buffer.WriteInt32(song.StreetShdDjPoint);

            buffer.WriteInt32(song.ClubEzActivation);
            buffer.WriteInt32(song.ClubEzExr);
            buffer.WriteInt32(song.ClubEzUnknown);
            buffer.WriteInt32(song.ClubEzNotes);
            buffer.WriteInt32(song.ClubEzUnlock);
            buffer.WriteInt32(song.ClubEzDjPoint);

            buffer.WriteInt32(song.ClubNmActivation);
            buffer.WriteInt32(song.ClubNmExr);
            buffer.WriteInt32(song.ClubNmUnknown);
            buffer.WriteInt32(song.ClubNmNotes);
            buffer.WriteInt32(song.ClubNmUnlock);
            buffer.WriteInt32(song.ClubNmDjPoint);

            buffer.WriteInt32(song.ClubHdActivation);
            buffer.WriteInt32(song.ClubHdExr);
            buffer.WriteInt32(song.ClubHdUnknown);
            buffer.WriteInt32(song.ClubHdNotes);
            buffer.WriteInt32(song.ClubHdUnlock);
            buffer.WriteInt32(song.ClubHdDjPoint);

            buffer.WriteInt32(song.ClubShdActivation);
            buffer.WriteInt32(song.ClubShdExr);
            buffer.WriteInt32(song.ClubShdUnknown);
            buffer.WriteInt32(song.ClubShdNotes);
            buffer.WriteInt32(song.ClubShdUnlock);
            buffer.WriteInt32(song.ClubShdDjPoint);
        }

        private SongCategoryType GetSongCategory(string category)
        {
            if (!int.TryParse(category, out int categoryNum))
            {
                return SongCategoryType.None;
            }

            if (!Enum.IsDefined(typeof(SongCategoryType), categoryNum))
            {
                return SongCategoryType.None;
            }

            return (SongCategoryType) categoryNum;
        }

        private string GetSongCategory(SongCategoryType category)
        {
            return ((int) category).ToString();
        }
    }
}