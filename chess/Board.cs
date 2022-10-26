using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public class Board
    {
        private int widthBoard;
        private int heightBoard;

        protected int _squareHeight = 50;
        protected int _squareWidth = 50;
        protected Square[,] _squares = new Square[8, 8];
        Form _parentForm;

        public Square[,] Squares
        {
            get { return _squares; }
        }

        public Board(Form ParentForm, int width = 400, int height = 400)
        {
            this._parentForm = ParentForm;

            this.widthBoard = width;
            this.heightBoard = height;
            init();
        }
        public void init()
        {
            int left = 0;
            int top = 0;
            SquareColor c = SquareColor.White;

            for (int i = 0; i < 8; i++)
            {
                left = 0;
                for (int j = 0; j < 8; j++)
                {
                    Square sq = new Square( i, j, c, this);
                    sq.Size = new System.Drawing.Size(_squareWidth, _squareHeight);
                    sq.Left = left;
                    sq.Top = top;

                    left += _squareWidth;
                    if (c == SquareColor.White) c = SquareColor.Black;
                    else c = SquareColor.White;

                    _squares[i, j] = sq;
                    _parentForm.Controls.Add(_squares[i, j]);
                }
                top += _squareHeight;
                if (c == SquareColor.White) c = SquareColor.Black;
                else c = SquareColor.White;
            }


            // add piece into board
            // King
            this[0, 4] = new King(_squares[0, 4], PieceColor.Black);
            this[7, 3] = new King(_squares[7, 3], PieceColor.White);
            // Queen
            this[0, 3] = new Queen(_squares[0, 3], PieceColor.Black);
            this[7, 4] = new Queen(_squares[7, 4], PieceColor.White);
            // Bishop
            this[0, 2] = new Bishop(_squares[0, 2], PieceColor.Black);
            this[0, 5] = new Bishop(_squares[0, 5], PieceColor.Black);
            this[7, 2] = new Bishop(_squares[7, 2], PieceColor.White);
            this[7, 5] = new Bishop(_squares[7, 5], PieceColor.White);
            // Knight
            this[0, 1] = new Knight(_squares[0, 1], PieceColor.Black);
            this[0, 6] = new Knight(_squares[0, 6], PieceColor.Black);
            this[7, 1] = new Knight(_squares[7, 1], PieceColor.White);
            this[7, 6] = new Knight(_squares[7, 6], PieceColor.White);
            // Rook
            this[0, 0] = new Rook(_squares[0, 0], PieceColor.Black);
            this[0, 7] = new Rook(_squares[0, 7], PieceColor.Black);
            this[7, 0] = new Rook(_squares[7, 0], PieceColor.White);
            this[7, 7] = new Rook(_squares[7, 7], PieceColor.White);
            // Pawn
            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new Pawn(_squares[1, i], PieceColor.Black);
            }
            for (int i = 0; i < 8; i++)
            {
                this[6, i] = new Pawn(_squares[6, i], PieceColor.White);
            }
        }

        // method add Piece
        public Piece this[int i, int j]
        {
            get { return _squares[i, j].Piece; }
            set { _squares[i, j].Piece = value; }
        }
        // select square
        protected Square _selectedSquare;
        public Square SelectedSquare
        {
            get { return _selectedSquare; }
            set 
            {
                _selectedSquare = value;
            }
        }
        protected bool _isSelecting = false;
        public bool IsSelecting
        {
            get { return _isSelecting; }
            set { _isSelecting = value; }
        }
        // method turn play
        protected PieceColor _turnPlay = PieceColor.White;
        // false: Black , true: White
        public PieceColor TurnPlay
        {
            get { return _turnPlay; }
            set { _turnPlay = value;}
        }
    }
}
