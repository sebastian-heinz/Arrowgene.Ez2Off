using System;

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class Rank
    {
        public Rank()
        {
            Id = -1;
        }

        public int Id { get; set; }
        public Game Game { get; set; }
        public Score Score { get; set; }
        public byte Ranking { get; set; }
        public TeamType Team { get; set; }
    }
}