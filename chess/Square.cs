using Chess;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public enum SquareColor { White, Black };
    public class Square : PictureBox
    {
        protected Board _board;
        public Board Boad
        {
            get { return _board; }
        }

        protected int _x;
        protected int _y;
        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }

        protected Piece _piece;
        public Piece Piece
        {
            get { return _piece; }
            set
            {
                _piece = value;
                if(_piece != null) Image = _piece.Image;
                else Image = null;
            }
        }

        protected SquareColor _color;
        public SquareColor color
        {
            get { return _color; }
            set
            {
                _color = value;
                if (_color == SquareColor.White)
                {
                    this.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    this.BackColor = System.Drawing.Color.SteelBlue;
                }
            }
        }
        protected bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;

                if(value == true)
                {
                    BackColor = System.Drawing.Color.SandyBrown;
                    _board.SelectedSquare = this;
                }
                else
                {
                    color = color;
                }
            }
        }
        public Square(int x, int y, SquareColor color, Board b, Piece p = null)
        {
            X = x;
            Y = y;

            this.color = color;
            SizeMode = PictureBoxSizeMode.StretchImage;

            this._board = b;
            this._piece = p;

            
            this.MouseDoubleClick += new MouseEventHandler(OnMouse_DoubleClick);
            this.MouseClick += new MouseEventHandler(OnMouse_Click);
        }

        private void OnMouse_DoubleClick(object sender, MouseEventArgs e)
        {
            Square sq = (Square)sender;
            Board board = sq._board;
            if (sq._board.IsSelecting == false && sq.Selected == false && sq._piece != null)
            {
                if(sq._board.TurnPlay == sq.Piece.Color)
                {
                    sq.Selected = true;
                    sq._board.IsSelecting = true;
                    //hiện ra những ô có thể đi
                    // tượng x
                    // hậu x
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (sq.Piece.checkValidMove(board.Squares[i, j].X, board.Squares[i, j].Y, sq.X, sq.Y, board.Squares[i, j]) == true)
                            {
                                if (board.Squares[i, j].color == SquareColor.Black)
                                    board.Squares[i, j].BackColor = System.Drawing.Color.SeaGreen;
                                else board.Squares[i, j].BackColor = System.Drawing.Color.LightGreen;
                                // những ô có thể bị ăn
                                if (board.Squares[i, j].Piece != null && board.Squares[i, j].Piece.Color != sq.Piece.Color)
                                {
                                    if (board.Squares[i, j].color == SquareColor.Black)
                                        board.Squares[i, j].BackColor = System.Drawing.Color.IndianRed;
                                    else board.Squares[i, j].BackColor = System.Drawing.Color.LightCoral;
                                }
                            }
                        }
                    }
                }
            }
            else if(sq._board.IsSelecting == true && sq.Selected == true && sq._piece != null)
            {
                sq.Selected = false;
                sq._board.IsSelecting = false;
                //xóa đi các ô đã gợi ý
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (sq.Piece.checkValidMove(board.Squares[i, j].X, board.Squares[i, j].Y, sq.X, sq.Y, board.Squares[i, j]) == true)
                        {
                            board.Squares[i, j].color = board.Squares[i, j].color;
                        }
                    }
                }
            }
        }

        private void OnMouse_Click(object sender, MouseEventArgs e)
        {
            Square sq = (Square)sender;
            if (sq._board.IsSelecting == true && sq.Selected == false)
            {
                Board board = sq._board;
                Square sqBefore = board.SelectedSquare;
                if (board.SelectedSquare.Piece.checkValidMove(sq.X, sq.Y, board.SelectedSquare.X, board.SelectedSquare.Y, sq) == true)
                {
                    if (board.SelectedSquare != null)
                    {
                        //xóa đi các ô đã gợi ý
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (sqBefore.Piece.checkValidMove(board.Squares[i, j].X, board.Squares[i, j].Y, sqBefore.X, sqBefore.Y, board.Squares[i, j]) == true)
                                {
                                    board.Squares[i, j].color = board.Squares[i, j].color;
                                }
                            }
                        }
                        //====================================
                        sq.Piece = board.SelectedSquare.Piece;
                        board.SelectedSquare.Piece = null;
                        board.SelectedSquare.Selected = false;

                        sq._board.IsSelecting = false;

                        // chuyển lượt
                        TurnLable.changeTextTurn();
                        if (sq._board.TurnPlay == PieceColor.White)
                        {
                            sq._board.TurnPlay = PieceColor.Black;
                        }
                        else
                        {
                            sq._board.TurnPlay = PieceColor.White;
                        }

                    }
                }
            }
        }
    }
}
