using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using chess;

namespace Chess
{
    static class TurnLable
    {
        public static Label lb_turn = new Label()
        {
            Text = "White",
            Location = new Point(438, 130)
    };
        public static void changeTextTurn()
        {
            if (lb_turn.Text == "White") lb_turn.Text = "Black";
            else lb_turn.Text = "White";
        }
    }
    internal class Program
    {
        class MyForm : Form
        {
            public MyForm()
            {
                this.Text = "Chess game";
                this.Size = new System.Drawing.Size(525, 438);

                Label lb1 = new Label();
                lb1.Text = "===TURN===";
                lb1.Location = new Point(420,100);

                Board bd = new Board(this);

                this.Controls.Add(lb1);
                this.Controls.Add(TurnLable.lb_turn);
            }
        }
        static void Main(string[] args)
        {
            MyForm form = new MyForm();
            Application.Run(form);
        }
    }
}
