using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace PockerAnalitics
{
    public class Player
    {
        //Winner устанавливается в true только у победителя
        public bool Winner { get; set; }
        public string Name { get; set; }
        public Bitmap Photo { get; set; }
        public Combinations Combination { get; set; }

        //поля PeakH и PeakL нужен для определения 
        //победителя при одинаковых комбинациях
        public int PeakL { get; set; }
        public int PeakH { get; set; }
        public List<Card> Cards;

        public Player()
        {
            Cards = new List<Card>();
            Winner = false;
        }
    }
}
