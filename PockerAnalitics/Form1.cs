using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PockerAnalitics
{
    public partial class Form1 : Form
    {
        public Game game;
        List<Player> players;
        List<Card> tableUsable;
        System.Windows.Forms.Timer timer;

        public Form1()
        {
            InitializeComponent();
        }

        //метод обработки нажатия Старт из меню
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game = new Game();
            startToolStripMenuItem.Enabled = false;
            initializeTable();
            players = new List<Player>();
            tableUsable = new List<Card>();
            game.Play(out players, out tableUsable);
            game.PerformEvalution();
            game.defineWinner();
            timerFunc();
        }

        private void initializeTable()
        {
            lblRes.Text = "";
            lblWinner.Text = "";
            string strPic = "../../images/backPng.png";
            picFlopOne.Image = new Bitmap(strPic);
            picFlopTwo.Image = new Bitmap(strPic);
            picFlopThree.Image = new Bitmap(strPic);
            picTurn.Image = new Bitmap(strPic);
            picRiver.Image = new Bitmap(strPic);
        }

        //таймер ежесекундно вызывающий timerAction
        private void timerFunc()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 50;
            timer.Tick += new EventHandler(timerAction);
            timer.Start();
        }

        //метод открывающий карты игрока и на столе
        private void timerAction(object sender, EventArgs e)
        {
            showPlayersCards();
            showTablesCards();
        }

        //вспомогательный метод открытия(присвоение) карт игрока
        private void showPlayersCards()
        {
            //foreach (var player in players)
            for (int i = 0; i < players.Count; i++)
            {
                switch (players[i].Name)
                {
                    case "Angela":
                        {
                            picDeOne.Image = players[i].Cards[0].Picture;
                            picDeTwo.Image = players[i].Cards[1].Picture;
                            break;
                        }
                    case "Barak":
                        {
                            picUsOne.Image = players[i].Cards[0].Picture;
                            picUsTwo.Image = players[i].Cards[1].Picture;
                            break;
                        }
                    case "Modi":
                        {
                            picInOne.Image = players[i].Cards[0].Picture;
                            picInTwo.Image = players[i].Cards[1].Picture;
                            break;
                        }
                    case "Vova":
                        {
                            picRuOne.Image = players[i].Cards[0].Picture;
                            picRuTwo.Image = players[i].Cards[1].Picture;
                            break;
                        }
                    case "Xi":
                        {
                            picCnOne.Image = players[i].Cards[0].Picture;
                            picCnTwo.Image = players[i].Cards[1].Picture;
                            break;
                        }
                    default:
                        break;
                }
            }

        }

        //вскрытие карт стола через паузу
        int pbCircle = 0;
        private void showTablesCards()
        {
            progressBar1.Value++;
            if (progressBar1.Value == progressBar1.Maximum)
            {
                pbCircle++;
                if (pbCircle == 1)
                    showFlop();
                if (pbCircle == 2)
                    showTurn();
                if (pbCircle == 3)
                {
                    showRiver();
                    timer.Stop();
                }
                progressBar1.Value = 0;
            }
        }

        //открытие позиции Ривер
        private void showRiver()
        {
            picRiver.Image = tableUsable[4].Picture;
            startToolStripMenuItem.Enabled = true;
            timer.Stop();
            foreach (var player in players)
            {
                lblRes.Text += player.Name + "=" + player.Combination + ",";
                if (player.Winner)
                    lblWinner.Text = player.Name + "'s " + player.Combination + " won !!!";
            }
            pbCircle = 0;
            game.Dispose();
        }

        //открытие позиции Тёрн
        private void showTurn()
        {
            picTurn.Image = tableUsable[3].Picture;
        }

        //открытие позиции Флоп
        private void showFlop()
        {
            picFlopOne.Image = tableUsable[0].Picture;
            picFlopTwo.Image = tableUsable[1].Picture;
            picFlopThree.Image = tableUsable[2].Picture;
        }

        //назначение времени для цикла в 10 сек.
        private void secToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 10;
        }

        //увеличение времени для цикла на 5 сек.
        private void secToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum += 5;
        }

        //сокращение времени для цикла на 5 сек.
        private void secToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (progressBar1.Maximum == 5)
            {
                MessageBox.Show("It is already a lowest value");
                return;
            }
            progressBar1.Maximum -= 5;
        }

        //остановка таймера и игры
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.Dispose();
            timer.Stop();
            progressBar1.Value = 0;
            startToolStripMenuItem.Enabled = true;
            pbCircle = 0;
        }

        //перезапускает цикл с уже открывшимися картами
        //для анализа определенных комбинвций
        private void btnTest_Click(object sender, EventArgs e)
        {
            lblRes.Text = "";
            timerFunc();
            game.PerformEvalution();
            game.defineWinner();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PockerAnalitics задумывалась как игра, которая бы определяла" +
                            " вероятность   выигрыша   каждого из   персонажей и коэфицент" +
                            " выигрыша для потенциальной ставки игрока. Пока не закончена.\nАвтор: jtahun@bk.ru",
                            "PockerAnalitics");
        }
    }
}
