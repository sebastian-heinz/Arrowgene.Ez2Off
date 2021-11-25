using System;

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class Ez2OnModelCard
    {
        public Ez2OnModelCard()
        {
            Id = -1;
        }

        public string Text { get; set; }
        public int Id { get; set; }
    }
}