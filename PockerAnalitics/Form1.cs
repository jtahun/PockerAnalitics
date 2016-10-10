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
        Deck deck = new Deck();
        PictureBox[] dto = new PictureBox[52];
        string back = "../../images/backPng.png";
        string suitNkind;
        PictureBox buffPic;


        int columnStep = 73;
        int rowStep = 98;

        public Form1()
        {
            InitializeComponent();
            InitializePicBox();

            int num = 0;
            int x = showBack();
            for (int i = 0; i < x; i++)
                this.Controls.Add(dto[num++]);
        }

        private void InitializePicBox()
        {
            PictureBox[] pBxs = new PictureBox[]{picBxOne1,picBxOne2,picBxFlop1,picBxFlop2,
                                                picBxFlop3,picBxTurn,picBxRiver,picBxTwo1,picBxTwo2};
            foreach (var pB in pBxs)
            {
                pB.AllowDrop = true;
                pB.MouseDown += pB_MouseDown;
                pB.DragEnter += pB_DragEnter;
                pB.DragDrop += pB_DragDrop;
            }
        }

        private int showBack()
        {
            int num = 0;
            for (int i = 0; i <= 294 / rowStep; i++)
            {
                for (int j = 0; j <= 876 / columnStep; j++)
                {
                    dto[num] = new PictureBox();
                    dto[num].Click += new EventHandler(cardClicked);
                    dto[num].DoubleClick += new EventHandler(othCardClicked);
                    dto[num].DragEnter += image_DragEnter;
                    dto[num].DragDrop += image_DragNdrop;
                    dto[num].MouseDown += card_MouseDown;
                    dto[num].BackColor = Color.Olive;
                    dto[num].AllowDrop = true;
                    dto[num].Location = new Point(j * columnStep, i * rowStep);
                    dto[num].Size = new Size(columnStep, rowStep);
                    dto[num].Image = new Bitmap(back);
                    dto[num++].SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            return num;
        }

        private void image_DragNdrop(object sender, DragEventArgs e)
        {
            var obj = (PictureBox)sender;
            obj.Image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
            obj.Name = suitNkind;
        }

        private void image_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = DragDropEffects.Move;
        }

        private void pB_MouseDown(object sender, MouseEventArgs e)
        {
            var pBx = (PictureBox)sender;
            suitNkind = pBx.Name;

            var img = pBx.Image;
            if (img == null) return;
            if (DoDragDrop(img, DragDropEffects.Move) == DragDropEffects.Move)
            {
                pBx.Image = null;
                pBx.Name = "";
            }

        }

        private void pB_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = DragDropEffects.Move;
        }

        private void pB_DragDrop(object sender, DragEventArgs e)
        {
            var Obj = (PictureBox)sender;
            Obj.Image = (Bitmap)e.Data.GetData(DataFormats.Bitmap, true);
        }

        private void card_MouseDown(object sender, MouseEventArgs e)
        {
            var obj = (PictureBox)sender;
            suitNkind = obj.Name;
            var pic = obj.Image;
            if (pic == null) return;
            if (DoDragDrop(pic, DragDropEffects.Move) == DragDropEffects.Move)
                obj.Image = null;
        }

        private void cardClicked(object sender, EventArgs e)
        {
            buffPic = (sender as PictureBox);
            string[] suitKind = buffPic.Name.Split(new Char[] { ' ' });
            //Card card = deck.getCard(suitKind[0], suitKind[1]);
            //buffPic.Image = card.Picture;
        }

        private void othCardClicked(object sender, EventArgs e)
        {
            PictureBox locPicbx = (sender as PictureBox);
            locPicbx.Name = buffPic.Name; buffPic.Name = "One";
            locPicbx.Image = buffPic.Image; buffPic.Image = new Bitmap(back);
        }

        private void showCards()
        {
            string suit_n_Kind = "";

            for (int i = 0; i < 52; i++)
            {
                dto[i].Image = deck.GetImage(out suit_n_Kind);
                dto[i].Name = suit_n_Kind;
            }
            Deck othDeck = new Deck();
            deck = othDeck;
        }

        private void btnNorm_Click(object sender, EventArgs e)
        {
            showCards();
        }

        private void btnShuffle_Click_1(object sender, EventArgs e)
        {
            deck.Shuffle();
            showCards();
        }
    }
}
