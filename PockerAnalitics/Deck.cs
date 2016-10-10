using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace PockerAnalitics
{
    //Класс - колода, содержит контейнер для объектов Card
    //и соответственно рисунков к ним
    public class Deck
    {
        private List<Card> _deck;
        private Random _rand;
        private List<Bitmap> _deckImgs;

        public Deck()
        {
            _deck = new List<Card>();
            _rand = new Random();
            _deckImgs = getDeciImgs();

            string[] suits = new string[] { "Clubs", "Spades", "Harts", "Diamonds" };
            string[] kinds = new string[] { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };

            int num = 0;
            //в циклах просходит наполнение объекта Deck данными
            foreach (var suit in suits)
                foreach (var kind in kinds)
                    _deck.Add(new Card { Suit = suit, Kind = kind, Picture = _deckImgs[num++] });
            //num++;
        }

        //метод использует рисунок для разъединения на отдельные
        //рисунки, возвращает их для присвоения объекту Card
        private List<Bitmap> getDeciImgs()
        {
            Bitmap genDeckImg = new Bitmap("../../images/deck.png");
            List<Bitmap> locBitmapList = new List<Bitmap>();
            int columnStep = 73;
            int rowStep = 98;

            for (int i = 0; i <= 294 / rowStep; i++)
            {
                for (int j = 0; j <= 876 / columnStep; j++)
                {
                    Rectangle rect = new Rectangle(j * columnStep, i * rowStep, columnStep, rowStep);
                    Bitmap aloneCard = (Bitmap)genDeckImg.Clone(rect, genDeckImg.PixelFormat);
                    locBitmapList.Add(aloneCard);
                }
            }
            return locBitmapList;
        }

        //метод для перетасовки обектов Card 
        public void Shuffle()
        {
            Card card;
            Random rand = new Random();
            List<Card> locDeck = new List<Card>(_deck);
            _deck.Clear();
            for (int i = locDeck.Count; i > 0; i--)
            {
                card = locDeck[rand.Next(locDeck.Count)];
                _deck.Add(card);
                locDeck.Remove(card);
            }
        }

        //метод возвращает картинку карты и удаляет ее из колоды
        public Bitmap GetImage()
        {
            Card card;
            int num = 0;
            card = this._deck[num];
            this._deck.Remove(card);
            return card.Picture;
        }

        //метод для раздачи карт игрокам и на стол
        public void Deal(List<Player> players, Table table)
        {
            //int num = 0;
            for (int i = 0; i <= 1; i++)
            {
                foreach (var player in players)
                {
                    playerDealCard(player);
                }
            }
            tableDealCard(table);
        }

        //вспомогательный метод для раздачи игрокам
        private void playerDealCard(Player player)
        {
            int cardNum = 0;
            Card card = _deck[cardNum];
            player.Cards.Add(card);
            _deck.Remove(card);
        }

        // //вспомогательный метод для раздачи на стол
        private void tableDealCard(Table table)
        {
            int cardNum = 0;
            Card card;
            for (int i = 0; i <= 7; i++)
            {
                card = _deck[cardNum++];
                table.tableCards.Add(card);
                _deck.Remove(card);
            }
            int[] nums = new int[] { 1, 2, 3, 5, 7 };
            foreach (var num in nums)
                table.useableCards.Add(table.tableCards[num]);
        }
    }
}
