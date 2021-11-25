using System;

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class Ez2OnModelItem
    {
        public Ez2OnModelItem()
        {
            Id = -1;
        }

        public string Image { get; set; }
        public int A { get; set; }
        public string Name { get; set; }
        public ItemCurrencyType Currency { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }
        public int Level { get; set; }

        /// <summary>
        /// 경험치
        /// </summary>
        public int ExpPlus { get; set; }
        public int CoinPlus { get; set; }
        public int HpPlus { get; set; }

        /// <summary>
        /// 회복력
        /// </summary>
        public int ResiliencePlus { get; set; }

        /// <summary>
        /// 방어력
        /// </summary>
        public int DefensePlus { get; set; }
        public int K { get; set; }
        public int L { get; set; }
        public int M { get; set; }
        public int N { get; set; }
        public int O { get; set; }
        public string Effect { get; set; }
        public int Id { get; set; }
        public int Enabled { get; set; }
        public ItemType Type { get; set; }
        public int S { get; set; }
        public int T { get; set; }
        public int U { get; set; }
        public int V { get; set; }
        public int DjPointPlus { get; set; }
        public int X { get; set; }

        public Item ToItem()
        {
            Item item = new Item();
            item.Image = Image;
            item.Name = Name;
            item.Duration = Duration;
            item.Price = Price;
            item.Level = Level;
            item.ExpPlus = ExpPlus;
            item.CoinPlus = CoinPlus;
            item.HpPlus = HpPlus;
            item.ResiliencePlus = ResiliencePlus;
            item.DefensePlus = DefensePlus;
            item.Effect = Effect;
            item.Id = Id;
            item.Type = Type;
            item.Currency = Currency;
            item.Enabled = Enabled != 0;
            return item;
        }
    }
}