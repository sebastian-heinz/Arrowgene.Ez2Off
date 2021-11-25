using System;

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class SongDetail
    {
        public ModeType Mode { get; set; }
        public DifficultyType Difficulty { get; set; }
        public int Exr { get; set; }
        public int Notes { get; set; }
        public int Activation { get; set; }
        public int Unknown { get; set; }
        public int Unlock { get; set; }
        public int DjPoint { get; set; }
    }
}