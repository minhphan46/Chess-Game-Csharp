using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public class ResourceScope
    {
        // BLACK
        public static Image IMAGE_PAWN_BLACK = Resources.pawn_black;
        public static Image IMAGE_ROOK_BLACK = Resources.rook_black;
        public static Image IMAGE_KNIGHT_BLACK = Resources.knight_black;
        public static Image IMAGE_BISHOP_BLACK = Resources.bishop_black;
        public static Image IMAGE_QUEEN_BLACK = Resources.queen_black;
        public static Image IMAGE_KING_BLACK = Resources.king_black;
        // WHITE
        public static Image IMAGE_PAWN_WHITE = Resources.pawn_white;
        public static Image IMAGE_ROOK_WHITE = Resources.rook_white;
        public static Image IMAGE_KNIGHT_WHITE = Resources.knight_white;
        public static Image IMAGE_BISHOP_WHITE = Resources.bishop_white;
        public static Image IMAGE_QUEEN_WHITE = Resources.queen_white;
        public static Image IMAGE_KING_WHITE = Resources.king_white;
    }
    public enum PieceColor { White, Black};
    public class Piece
    {
        protected List<KeyValuePair<int, int>> _squareCanMove;
        public List<KeyValuePair<int, int>> SquareCanMove
        {
            get { return _squareCanMove; }
            set { _squareCanMove = value; }
        }
        protected Square _square;
        public Square Square
        {
            get { return _square; }
        }

        protected Image _image;
        public Image Image
        {
            get { return _image; }
        }

        protected PieceColor _color;
        public PieceColor Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public Piece(Square sq, PieceColor color)
        {
            _color = color;
            _square = sq;
        }
        public virtual bool checkValidMove(int xAfter, int yAfter, int xBefore, int yBefore, Square sq)
        {
            return false;
        }
    }
    public class King : Piece
    {
        public King(Square sq, PieceColor color) : base(sq,color)
        {
            if (color == PieceColor.White)
                _image = ResourceScope.IMAGE_KING_WHITE;
            else
                _image = ResourceScope.IMAGE_KING_BLACK;
        }
        public override bool checkValidMove(int xAfter, int yAfter, int xBefore, int yBefore, Square sq)
        {
            if( sq.Piece == null || sq.Piece.Color != this.Color)
            {
                if (xAfter == xBefore - 1 && (yAfter == yBefore - 1 || yAfter == yBefore || yAfter == yBefore + 1))
                {
                    return true;
                }
                if (xAfter == xBefore && (yAfter == yBefore - 1 || yAfter == yBefore || yAfter == yBefore + 1))
                {
                    return true;
                }
                if (xAfter == xBefore + 1 && (yAfter == yBefore - 1 || yAfter == yBefore || yAfter == yBefore + 1))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Pawn : Piece
    {
        public Pawn(Square sq, PieceColor color) : base(sq, color)
        {
            if (color == PieceColor.White)
                _image = ResourceScope.IMAGE_PAWN_WHITE;
            else
                _image = ResourceScope.IMAGE_PAWN_BLACK;
        }
        public override bool checkValidMove(int xAfter, int yAfter, int xBefore, int yBefore, Square sq)
        {
            if (sq.Piece == null)
            {
                if (this.Color == PieceColor.Black)
                {
                    if (xAfter == xBefore + 1 && (yAfter == yBefore)) return true;
                    if ((xAfter == xBefore + 2) && (yAfter == yBefore) && sq.Boad.Squares[xBefore+1, yBefore].Piece == null)
                    {
                        return true;
                    }
                }
                else // white
                {
                    if (xAfter == xBefore - 1 && (yAfter == yBefore)) return true;
                    if ((xAfter == xBefore - 2) && (yAfter == yBefore) && sq.Boad.Squares[xBefore - 1, yBefore].Piece == null)
                    {
                        return true;
                    }
                }
            }
            else if (sq.Piece.Color != this.Color) //  xử lý ăn tướng của con tốt
            {
                if (this.Color == PieceColor.Black)
                {
                    if ((xAfter == xBefore + 1 && yAfter == yBefore - 1) || (xAfter == xBefore + 1 && yAfter == yBefore + 1))
                    {
                        return true;
                    }
                }
                else // white
                {
                    if ((xAfter == xBefore - 1 && yAfter == yBefore - 1) || (xAfter == xBefore - 1 && yAfter == yBefore + 1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class Queen : Piece
    {
        public Queen(Square sq, PieceColor color) : base(sq, color)
        {
            if (color == PieceColor.White)
                _image = ResourceScope.IMAGE_QUEEN_WHITE;
            else
                _image = ResourceScope.IMAGE_QUEEN_BLACK;
        }
        public override bool checkValidMove(int xAfter, int yAfter, int xBefore, int yBefore, Square sq)
        {
            this.SquareCanMove = findListSquareCanMove(xAfter, yAfter, xBefore, yBefore, sq);
            foreach (KeyValuePair<int, int> sqInLists in this.SquareCanMove)
            {
                if (xAfter == sqInLists.Key && yAfter == sqInLists.Value)
                    return true;
            }
            /*if (sq.Piece == null || sq.Piece.Color != this.Color)
            {
                // duong thang
                if (xAfter == xBefore || yAfter == yBefore) return true;
                // duong cheo
                int a = Math.Abs(xAfter - xBefore);
                int b = Math.Abs(yAfter - yBefore);
                if (a == b) return true;
            }*/
            return false;
        }
        public List<KeyValuePair<int, int>> findListSquareCanMove(int xAfter, int yAfter, int xBefore, int yBefore, Square sq)
        {
            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
            Board b = sq.Boad;
            // check duong cheo
            // goc trai tren
            for (int i = xBefore - 1; i >= 0; i--)
            {
                for (int j = yBefore - 1; j >= 0; j--)
                {
                    int xSub = Math.Abs(i - xBefore);
                    int ysub = Math.Abs(j - yBefore);
                    if (xSub == ysub)
                    {
                        if (b.Squares[i, j].Piece == null)
                            list.Add(new KeyValuePair<int, int>(i, j));
                        else if (b.Squares[i, j].Piece.Color == b.SelectedSquare.Piece.Color)
                            goto DoneLoop1;
                        else if (b.Squares[i, j].Piece.Color != b.SelectedSquare.Piece.Color)
                        {
                            list.Add(new KeyValuePair<int, int>(i, j));
                            goto DoneLoop1;
                        }
                    }
                }
            }
            DoneLoop1:
            // goc phai tren
            for (int i = xBefore - 1; i >= 0; i--)
            {
                for (int j = yBefore + 1; j < 8; j++)
                {
                    int xSub = Math.Abs(i - xBefore);
                    int ysub = Math.Abs(j - yBefore);
                    if (xSub == ysub)
                    {
                        if (b.Squares[i, j].Piece == null)
                            list.Add(new KeyValuePair<int, int>(i, j));
                        else if (b.Squares[i, j].Piece.Color == b.SelectedSquare.Piece.Color)
                            goto DoneLoop2;
                        else if (b.Squares[i, j].Piece.Color != b.SelectedSquare.Piece.Color)
                        {
                            list.Add(new KeyValuePair<int, int>(i, j));
                            goto DoneLoop2;
                        }
                    }
                }
            }
            DoneLoop2:
            // ben trai duoi
            for (int i = xBefore + 1; i < 8; i++)
            {
                for (int j = yBefore - 1; j >= 0; j--)
                {
                    int xSub = Math.Abs(i - xBefore);
                    int ysub = Math.Abs(j - yBefore);
                    if (xSub == ysub)
                    {
                        if (b.Squares[i, j].Piece == null)
                            list.Add(new KeyValuePair<int, int>(i, j));
                        else if (b.Squares[i, j].Piece.Color == b.SelectedSquare.Piece.Color)
                            goto DoneLoop3;
                        else if (b.Squares[i, j].Piece.Color != b.SelectedSquare.Piece.Color)
                        {
                            list.Add(new KeyValuePair<int, int>(i, j));
                            goto DoneLoop3;
                        }
                    }
                }
            }
            DoneLoop3:
            // ben trai
            for (int i = xBefore + 1; i < 8; i++)
            {
                for (int j = yBefore + 1; j < 8; j++)
                {
                    int xSub = Math.Abs(i - xBefore);
                    int ysub = Math.Abs(j - yBefore);
                    if (xSub == ysub)
                    {
                        if (b.Squares[i, j].Piece == null)
                            list.Add(new KeyValuePair<int, int>(i, j));
                        else if (b.Squares[i, j].Piece.Color == b.SelectedSquare.Piece.Color)
                            goto DoneLoop4;
                        else if (b.Squares[i, j].Piece.Color != b.SelectedSquare.Piece.Color)
                        {
                            list.Add(new KeyValuePair<int, int>(i, j));
                            goto DoneLoop4;
                        }
                    }
                }
            }
            DoneLoop4:
            // check duong ngang, doc
            // xuong duoi
            for (int i = xBefore + 1; i < 8; i++)
            {
                if (b.Squares[i, yBefore].Piece == null)
                    list.Add(new KeyValuePair<int, int>(i, yBefore));
                else if (b.Squares[i, yBefore].Piece.Color == b.SelectedSquare.Piece.Color)
                    break;
                else if (b.Squares[i, yBefore].Piece.Color != b.SelectedSquare.Piece.Color)
                {
                    list.Add(new KeyValuePair<int, int>(i, yBefore));
                    break;
                }
            }
            // len
            for (int i = xBefore - 1; i >= 0; i--)
            {
                if (b.Squares[i, yBefore].Piece == null)
                    list.Add(new KeyValuePair<int, int>(i, yBefore));
                else if (b.Squares[i, yBefore].Piece.Color == b.SelectedSquare.Piece.Color)
                    break;
                else if (b.Squares[i, yBefore].Piece.Color != b.SelectedSquare.Piece.Color)
                {
                    list.Add(new KeyValuePair<int, int>(i, yBefore));
                    break;
                }
            }
            // ben phai
            for (int j = yBefore + 1; j < 8; j++)
            {
                if (b.Squares[xBefore, j].Piece == null)
                    list.Add(new KeyValuePair<int, int>(xBefore, j));
                else if (b.Squares[xBefore, j].Piece.Color == b.SelectedSquare.Piece.Color)
                    break;
                else if (b.Squares[xBefore, j].Piece.Color != b.SelectedSquare.Piece.Color)
                {
                    list.Add(new KeyValuePair<int, int>(xBefore, j));
                    break;
                }
            }
            // ben trai
            for (int j = yBefore - 1; j >= 0; j--)
            {
                if (b.Squares[xBefore, j].Piece == null)
                    list.Add(new KeyValuePair<int, int>(xBefore, j));
                else if (b.Squares[xBefore, j].Piece.Color == b.SelectedSquare.Piece.Color)
                    break;
                else if (b.Squares[xBefore, j].Piece.Color != b.SelectedSquare.Piece.Color)
                {
                    list.Add(new KeyValuePair<int, int>(xBefore, j));
                    break;
                }
            }
            return list;
        }
    }

    public class Rook : Piece
    {
        public Rook(Square sq, PieceColor color) : base(sq, color)
        {
            if (color == PieceColor.White)
                _image = ResourceScope.IMAGE_ROOK_WHITE;
            else
                _image = ResourceScope.IMAGE_ROOK_BLACK;

        }
        public override bool checkValidMove(int xAfter, int yAfter, int xBefore, int yBefore, Square sq)
        {
            this.SquareCanMove = findListSquareCanMove(xAfter, yAfter, xBefore, yBefore, sq);
            foreach(KeyValuePair<int, int> sqInLists in this.SquareCanMove)
            {
                if(xAfter == sqInLists.Key && yAfter == sqInLists.Value)
                    return true;
            }
            /*if (sq.Piece == null || sq.Piece.Color != this.Color)
            {
                // duong thang
                if (xAfter == xBefore || yAfter == yBefore) return true;
            }*/
            return false;
        }

        public List<KeyValuePair<int, int>> findListSquareCanMove(int xAfter, int yAfter, int xBefore, int yBefore, Square sq)
        {
            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
            Board b = sq.Boad;
            // xuong duoi
            for (int i = xBefore + 1; i < 8; i++)
            {
                if (b.Squares[i, yBefore].Piece == null)
                    list.Add(new KeyValuePair<int, int>(i, yBefore));
                else if (b.Squares[i, yBefore].Piece.Color == b.SelectedSquare.Piece.Color)
                    break;
                else if (b.Squares[i, yBefore].Piece.Color != b.SelectedSquare.Piece.Color)
                {
                    list.Add(new KeyValuePair<int, int>(i, yBefore));
                    break;
                }
            }
            // len
            for (int i = xBefore-1; i >= 0; i--)
            {
                if (b.Squares[i, yBefore].Piece == null)
                    list.Add(new KeyValuePair<int, int>(i, yBefore));
                else if (b.Squares[i, yBefore].Piece.Color == b.SelectedSquare.Piece.Color)
                    break;
                else if (b.Squares[i, yBefore].Piece.Color != b.SelectedSquare.Piece.Color)
                {
                    list.Add(new KeyValuePair<int, int>(i, yBefore));
                    break;
                }
            }
            // ben phai
            for (int j = yBefore + 1; j < 8; j++)
            {
                if (b.Squares[xBefore,j].Piece == null)
                    list.Add(new KeyValuePair<int, int>(xBefore, j));
                else if (b.Squares[xBefore, j].Piece.Color == b.SelectedSquare.Piece.Color)
                    break;
                else if (b.Squares[xBefore, j].Piece.Color != b.SelectedSquare.Piece.Color)
                {
                    list.Add(new KeyValuePair<int, int>(xBefore, j));
                    break;
                }
            }
            // ben trai
            for (int j = yBefore-1; j >= 0; j--)
            {
                if (b.Squares[xBefore, j].Piece == null)
                    list.Add(new KeyValuePair<int, int>(xBefore, j));
                else if (b.Squares[xBefore, j].Piece.Color == b.SelectedSquare.Piece.Color)
                    break;
                else if (b.Squares[xBefore, j].Piece.Color != b.SelectedSquare.Piece.Color)
                {
                    list.Add(new KeyValuePair<int, int>(xBefore, j));
                    break;
                }
            }
            return list;
        }
    }

    public class Knight : Piece
    {
        public Knight(Square sq, PieceColor color) : base(sq, color)
        {
            if (color == PieceColor.White)
                _image = ResourceScope.IMAGE_KNIGHT_WHITE;
            else
                _image = ResourceScope.IMAGE_KNIGHT_BLACK;
        }
        public override bool checkValidMove(int xAfter, int yAfter, int xBefore, int yBefore, Square sq)
        {
            if (sq.Piece == null || sq.Piece.Color != this.Color)
            {
                int a = Math.Abs(xAfter - xBefore);
                int b = Math.Abs(yAfter - yBefore);
                if ((a == 2 && b == 1) || ( a == 1 && b == 2)) return true;
            }
            return false;
        }
    }

    public class Bishop : Piece
    {
        public Bishop(Square sq, PieceColor color) : base(sq, color)
        {
            if (color == PieceColor.White)
                _image = ResourceScope.IMAGE_BISHOP_WHITE;
            else
                _image = ResourceScope.IMAGE_BISHOP_BLACK;
        }
        public override bool checkValidMove(int xAfter, int yAfter, int xBefore, int yBefore, Square sq)
        {
            this.SquareCanMove = findListSquareCanMove(xAfter, yAfter, xBefore, yBefore, sq);
            foreach (KeyValuePair<int, int> sqInLists in this.SquareCanMove)
            {
                if (xAfter == sqInLists.Key && yAfter == sqInLists.Value)
                    return true;
            }
            /*if (sq.Piece == null || sq.Piece.Color != this.Color)
            {
                int a = Math.Abs(xAfter - xBefore);
                int b = Math.Abs(yAfter - yBefore);
                if (a == b) return true;
            }*/
            return false;
        }
        public List<KeyValuePair<int, int>> findListSquareCanMove(int xAfter, int yAfter, int xBefore, int yBefore, Square sq)
        {
            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
            Board b = sq.Boad;
            // goc trai tren
            for(int i = xBefore-1; i >= 0; i--)
            {
                for( int j = yBefore-1; j >= 0; j--)
                {
                    int xSub = Math.Abs(i - xBefore);
                    int ysub = Math.Abs(j - yBefore);
                    if (xSub == ysub)
                    {
                        if (b.Squares[i, j].Piece == null)
                            list.Add(new KeyValuePair<int, int>(i, j));
                        else if (b.Squares[i, j].Piece.Color == b.SelectedSquare.Piece.Color)
                            goto DoneLoop1;
                        else if (b.Squares[i, j].Piece.Color != b.SelectedSquare.Piece.Color)
                        {
                            list.Add(new KeyValuePair<int, int>(i, j));
                            goto DoneLoop1;
                        }
                    }
                }
            }
            DoneLoop1:
            // goc phai tren
            for (int i = xBefore - 1; i >= 0; i--)
            {
                for (int j = yBefore + 1; j < 8; j++)
                {
                    int xSub = Math.Abs(i - xBefore);
                    int ysub = Math.Abs(j - yBefore);
                    if (xSub == ysub)
                    {
                        if (b.Squares[i, j].Piece == null)
                            list.Add(new KeyValuePair<int, int>(i, j));
                        else if (b.Squares[i, j].Piece.Color == b.SelectedSquare.Piece.Color)
                            goto DoneLoop2;
                        else if (b.Squares[i, j].Piece.Color != b.SelectedSquare.Piece.Color)
                        {
                            list.Add(new KeyValuePair<int, int>(i, j));
                            goto DoneLoop2;
                        }
                    }
                }
            }
            DoneLoop2:
            // ben trai duoi
            for (int i = xBefore + 1; i < 8; i++)
            {
                for (int j = yBefore - 1; j >= 0; j--)
                {
                    int xSub = Math.Abs(i - xBefore);
                    int ysub = Math.Abs(j - yBefore);
                    if (xSub == ysub)
                    {
                        if (b.Squares[i, j].Piece == null)
                            list.Add(new KeyValuePair<int, int>(i, j));
                        else if (b.Squares[i, j].Piece.Color == b.SelectedSquare.Piece.Color)
                            goto DoneLoop3;
                        else if (b.Squares[i, j].Piece.Color != b.SelectedSquare.Piece.Color)
                        {
                            list.Add(new KeyValuePair<int, int>(i, j));
                            goto DoneLoop3;
                        }
                    }
                }
            }
            DoneLoop3:
            // ben trai
            for (int i = xBefore + 1; i < 8; i++)
            {
                for (int j = yBefore + 1; j < 8; j++)
                {
                    int xSub = Math.Abs(i - xBefore);
                    int ysub = Math.Abs(j - yBefore);
                    if (xSub == ysub)
                    {
                        if (b.Squares[i, j].Piece == null)
                            list.Add(new KeyValuePair<int, int>(i, j));
                        else if (b.Squares[i, j].Piece.Color == b.SelectedSquare.Piece.Color)
                            goto DoneLoop4;
                        else if (b.Squares[i, j].Piece.Color != b.SelectedSquare.Piece.Color)
                        {
                            list.Add(new KeyValuePair<int, int>(i, j));
                            goto DoneLoop4;
                        }
                    }
                }
            }
            DoneLoop4:
            return list;
        }
    }
}
