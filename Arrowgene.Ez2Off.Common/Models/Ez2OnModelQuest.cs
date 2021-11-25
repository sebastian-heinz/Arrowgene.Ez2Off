using System;

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class Ez2OnModelQuest
    {
        public int Id { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public string Title { get; set; }
        public string Mission { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public int K { get; set; }
        public int L { get; set; }
        public int M { get; set; }
        public int N { get; set; }
        public int O { get; set; }
        public int P { get; set; }
        public int Q { get; set; }
        public int R { get; set; }
        public int S { get; set; }
        public int T { get; set; }
        public int U { get; set; }
        public int V { get; set; }
        public int W { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Z1 { get; set; }
        public int Z2 { get; set; }
        public int Z3 { get; set; }
        public int Z4 { get; set; }
        public int Z5 { get; set; }
        public int Z6 { get; set; }
        public int Z7 { get; set; }
        public int Z8 { get; set; }
        public int Z9 { get; set; }
        public int Z10 { get; set; }
        public int Z11 { get; set; }

        public Quest ToQuest()
        {
            Quest quest = new Quest();
            quest.Id = Id;
            quest.Title = Title;
            quest.Mission = Mission;
            return quest;
        }
    }
}