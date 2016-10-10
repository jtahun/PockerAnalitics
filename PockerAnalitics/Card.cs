using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace PockerAnalitics
{
    public class Card
    {
        //в полях хранятся поля для Масти, Номинала и Картики для карты
        public string Suit { get; set; }
        public string Kind { get; set; }
        public Bitmap Picture { get; set; }

        //возвращает числовое значение карты
        public int CardValue()
        {
            int value = 0;
            switch (this.Kind)
            {
                case "Jack":
                    value = 11;
                    break;
                case "Queen":
                    value = 12;
                    break;
                case "King":
                    value = 13;
                    break;
                case "Ace":
                    value = 14;
                    break;
                default:
                    value = int.Parse(this.Kind);
                    break;
            }
            return value;
        }
    }
}
