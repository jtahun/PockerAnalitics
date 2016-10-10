using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace PockerAnalitics
{
    public class Deck
    {
        private List<Card> _deck;
        private List<Card> _dubDeck;
        private Random _rand;
        private List<Bitmap> _deckImgs;

        public Deck()
        {
            _deck = new List<Card>();
            _dubDeck = new List<Card>();
            _rand = new Random();
            _deckImgs = getDeciImgs();

            string[] suits = new string[] { "Clubs", "Spades", "Harts", "Diamonds" };
            string[] kinds = new string[] { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };

            int num = 0;
            foreach (var suit in suits)
                foreach (var kind in kinds)
                    _deck.Add(new Card { Suit = suit, Kind = kind, Picture = _deckImgs[num++] });
            //num++;
        }

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

        public Bitmap GetImage(out string suitNkind)
        {
            Card card;
            int num = 0;
            card = this._deck[num];
            suitNkind = "";
            suitNkind += card.Suit + " " + card.Kind + " ";
            this._deck.Remove(card);
            _dubDeck.Add(card);
            return card.Picture;
        }

        public void gatherDeck()
        {
            _deck = _dubDeck;
        }

        public Card getCard(string suit, string kind)
        {
            foreach (var card in _deck)
                if (card.Suit == suit && card.Kind == kind)
                    return card;
            return _deck[0];
        }
    }
}
