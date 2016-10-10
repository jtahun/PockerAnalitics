using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PockerAnalitics
{
    //класс для хранения объектов Card для стола
    public class Table
    {
        public List<Card> tableCards;
        public List<Card> useableCards;

        public Table()
        {
            tableCards = new List<Card>();
            useableCards = new List<Card>();
        }
    }
}
