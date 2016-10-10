using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace PockerAnalitics
{
    //класс Game содержит контейнер для карт игрока 
    // и для карт выкладываемых на стол
    public class Game : IDisposable
    {
        private List<Player> _players = new List<Player>();
        private Table _table = new Table();
        int[] arrOftc;

        //в конструкторе происходит инициализация игроков и 
        //добавка их в контейнер.
        public Game()
        {
            string[] names = new string[] { "Angela", "Barak", "Modi", "Vova", "Xi" };
            foreach (var name in names)
            {
                string forPhoto = String.Format("../../images/{0}.jpeg", name);
                _players.Add(new Player() { Name = name, Photo = new Bitmap(forPhoto), Combination = Combinations.None });
            }
            _table = new Table();
        }

        //в методе Play создается объект колоды
        //перетусовка карт колоды и раздача карт игрокам  и на стол
        public void Play(out List<Player> players, out List<Card> tableUsable)
        {
            Deck deck = new Deck();
            deck.Shuffle();
            deck.Deal(_players, _table);

            players = _players;
            tableUsable = _table.useableCards;
            feelTableArr(tableUsable);
        }

        //данный метод является однотипной операцией 
        //используемой в других методах
        private void feelTableArr(List<Card> tableUsable)
        {
            arrOftc = new int[]{0,0,tableUsable[0].CardValue(),
                                    tableUsable[1].CardValue(),
                                    tableUsable[2].CardValue(),
                                    tableUsable[3].CardValue(),
                                    tableUsable[4].CardValue()};
        }

        //в данном методе начинается поэтапные действия
        //по распознованию комбинации
        public void PerformEvalution()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                Player player = _players[i];
                //new Thread(new ParameterizedThreadStart(defineCombination)).Start(player);
                defineCombination(player);
            }
        }

        //следующий этап по распознованию, в general определяются
        //большенство комбинаций, кроме Стит и Флэш, которые определяются
        //в методах isStraight и Flush
        private void defineCombination(object obj)
        {
            general(obj);
            isStraight(obj);
            isFlush(obj);
        }

        //просходит анализ комбинаций и определение победителя
        public void defineWinner()
        {
            //в массиве winner хранятся комбинация, индекс игрока, 
            //поля пики Макс и Мин предыдущего игрока для сравнения 
            //с данными текущего игрока
            //0 - combination; 1 - index of a player; 2 - peakH; 3 - peakL;
            int[] winner = new int[] { 0, 0, 0, 0 };
            Player player;
            for (int i = 0; i < _players.Count(); i++)
            {
                player = _players[i];
                if (winner[0] == (int)player.Combination)
                {
                    if (winner[2] == player.PeakH)
                    {
                        winner[1] = (winner[3] > player.PeakL) ? winner[1] : i;
                        winner[3] = player.PeakL;
                    }
                    if (winner[2] < player.PeakH)
                    {
                        winner[2] = player.PeakH;
                        winner[1] = i;
                    }

                }
                if (winner[0] < (int)player.Combination)
                {
                    winner[0] = (int)player.Combination;
                    winner[1] = i;
                    winner[2] = player.PeakH;
                    winner[3] = player.PeakL;
                }
            }
            cleanPrevWins();
            player = _players[winner[1]];
            player.Winner = true;
        }

        private void cleanPrevWins()
        {
            foreach (var player in _players)
                player.Winner = false;
        }

        //в масив arrOftc добавляются карты игрока
        //сформированый масив и поля игока передаются в 
        //метод markParallel, который метит совпадения
        //и передает в следующий метод.
        private void general(object obj)
        {
            Player player = (obj as Player);
            arrOftc[0] = player.Cards[0].CardValue();
            arrOftc[1] = player.Cards[1].CardValue();

            int peakH = 0;
            int peakL = 0;
            Combinations comb = Combinations.None;
            markParallelArr(out peakH, out peakL, out comb);
            player.Combination = comb;
            player.PeakH = peakH;
            player.PeakL = peakL;
        }

        //данный метод метит совпадения в массиве карт - игрок и стол
        //в параллельный массив
        private void markParallelArr(out int H,
                                      out int L,
                                      out Combinations comb)
        {
            H = L = 0;
            comb = Combinations.None;

            int[] detectArr = new int[] { -1, -1, -1, -1, -1, -1, -1 };

            for (int n = 0; n < arrOftc.Length - 1; n++)
                for (int i = n; i < arrOftc.Length; i++)
                {
                    if (arrOftc[n] == arrOftc[i] && detectArr[i] == -1)
                        detectArr[i] = n;
                }
            countCoincedence(detectArr, out H, out L, out comb);
        }

        //данный метод производит подсчет совпадений в сформированном 
        //масиве и передает в метод который определяет комбинацию
        private void countCoincedence(int[] detectArr,
                                  out int H,
                                  out int L,
                                  out Combinations comb)
        {
            H = L = 0;
            comb = Combinations.None;
            int[] countArr = new int[] { 0, 0, 0, 0, 0, 0, 0 };
            foreach (int val in detectArr)
                //{
                try
                {
                    countArr[val]++;
                }
                catch
                {
                    countArr[6] = -1;
                }
            //}
            defineGeneral(countArr, out H, out L, out comb);
        }

        //метод по определению комбинации и инициализирует 
        //промежуточные переменные, присваемые игроку
        private void defineGeneral(int[] countArr, out int H, out int L, out Combinations comb)
        {
            H = L = 0;
            comb = Combinations.None;

            int pairH = 0;
            bool set = false;
            bool fullhouse = false;
            bool care = false;
            bool pair = false;

            //в цикле анализируется по кол-ву совпадений
            //каждая позиция для определения комбинации
            for (int i = 0; i < countArr.Length; i++)
            {
                //определение комбинации "пара" или " две пары"
                if (countArr[i] == 2)
                {
                    pairH = (arrOftc[i] > pairH) ? arrOftc[i] : pairH;
                    pair = true;
                    if (countArr[i] == 2 && arrOftc[i] > L
                        && !set && !fullhouse && !care)
                    {
                        L = (arrOftc[i] > L && arrOftc[i] > H) ? H : arrOftc[i];
                        if (arrOftc[i] > H && !fullhouse && !care && !set)
                            H = arrOftc[i];
                        comb = (L == H || L == 0) ? Combinations.Pair : Combinations.TwinPair;
                    }
                }

                //определие комбинации "Сет"
                if (countArr[i] == 3 && L == 0)
                {
                    set = true;
                    H = arrOftc[i];
                    if (arrOftc[0] != arrOftc[1])
                        L = (arrOftc[0] != arrOftc[i]
                               && arrOftc[1] != arrOftc[i]
                               && arrOftc[0] > arrOftc[1]) ? arrOftc[0] : arrOftc[1];
                    comb = Combinations.Set;
                }

                //определение комбинации "ФулХаус"
                if (set && pair)
                {
                    fullhouse = true;
                    set = false;
                    L = pairH;
                    comb = Combinations.Fullhouse;
                }

                //определение комбинации "Карэ"
                if (countArr[i] == 4)
                {
                    care = true;
                    H = arrOftc[i];
                    L = (arrOftc[0] != arrOftc[i]
                                && arrOftc[1] != arrOftc[i]
                                && arrOftc[0] > arrOftc[1]) ? arrOftc[0] : arrOftc[1];
                    comb = Combinations.Care;
                }
            }

        }

        // метод определения комбинации "Флэш"
        private void isFlush(object obj)
        {
            Player player = (obj as Player);
            int[] intFlush = new int[] { 0, 0, 0, 0 };
            string[] strFlush = new string[] {player.Cards[0].Suit,
                                              player.Cards[1].Suit,
                                              _table.useableCards[0].Suit,
                                              _table.useableCards[1].Suit,
                                              _table.useableCards[2].Suit,
                                              _table.useableCards[3].Suit,
                                              _table.useableCards[4].Suit};

            intFlush = countQuantityOfSuits(strFlush);

            for (int i = 0; i < intFlush.Length; i++)
            {
                if (intFlush[i] >= 5)
                    player.Combination = Combinations.Flush;
            }
        }

        //вспомогательный метод для "Флэш"
        private int[] countQuantityOfSuits(string[] strFlush)
        {
            int[] arrOfSuits = new int[] { 0, 0, 0, 0 };
            for (int i = 0; i < strFlush.Length; i++)
            {
                switch (strFlush[i])
                {
                    case "Clubs":
                        arrOfSuits[0]++;
                        break;
                    case "Spades":
                        arrOfSuits[1]++;
                        break;
                    case "Harts":
                        arrOfSuits[2]++;
                        break;
                    case "Diamonds":
                        arrOfSuits[3]++;
                        break;
                    default:
                        break;
                }
            }
            return arrOfSuits;
        }

        // метод определения комбинации "Стрит"
        private void isStraight(object obj)
        {
            Player player = (obj as Player);
            int[] cardArr = new int[] { 0, 0, 0, 0, 0, 0, 0 };
            Array.Copy(arrOftc, cardArr, 7);
            Array.Sort(cardArr);
            bool aceAsOne = (cardArr[6] == 14) ? true : false;
            int peak = 0;
            int sequence = staightLogic(cardArr, aceAsOne, out peak);
            if (sequence >= 4)
            {
                player.Combination = Combinations.Straight;
                player.PeakH = peak;
            }
        }

        //вспомогательный метод для "Стрит"
        private int staightLogic(int[] cardArr, bool aceAsOne, out int peak)
        {
            int coinc = 0;
            peak = 0;
            for (int i = 0; i < (cardArr.Count() - 1); i++)
            {
                if (cardArr[i] + 1 == cardArr[i + 1])
                {
                    coinc++;
                    peak = cardArr[i + 1];
                    if (coinc == 3 && peak == 5 && aceAsOne)
                        coinc = 4;
                }
                else if (cardArr[i] == cardArr[i + 1])
                    coinc = coinc * 1;
                else
                    coinc = (coinc >= 4) ? 4 : 0;
            }
            return coinc;
        }

        //деструктор класса Game
        public void Dispose()
        {
            //Dispose(true);
            GC.SuppressFinalize(this);

        }
    }    
}
