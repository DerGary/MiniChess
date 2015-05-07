using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    public class GameBoard
    {
        private char[,] board = new char[Program.MAXROW, Program.MAXCOLUMN];

        /// <summary>
        /// Initializes a new GameBoard with the given string as pieces on the board
        /// </summary>
        /// <param name="s">a string to initialize the GameBoard. It should look like this: "nkqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK"</param>
        public GameBoard(string s)
        {
            MakeBoard(s);
        }

        public GameBoard(GameBoard gameBoard)
        {
            for (int a = 0; a < Program.MAXROW; a++)
            {
                for (int b = 0; b < Program.MAXCOLUMN; b++)
                {
                    board[a, b] = gameBoard.board[a, b];
                }
            }
        }

        /// <summary>
        /// Initializes a new GameBoard with the given string as pieces on the board.
        /// The pieces are saved in descending order so the Move Search Algorithm works.
        /// The Real saved Board can be printed by using ToStringReal()
        /// </summary>
        /// <param name="s">a string to initialize the GameBoard. It should look like this: "nkqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK"</param>
        private void MakeBoard(string s)
        {
            int a = Program.MAXROW - 1;
            int b = Program.MAXCOLUMN - 1;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != '\n' && s[i] != '\r')
                {
                    board[a, b] = s[i];
                    b--;
                    if (b == -1)
                    {
                        b = Program.MAXCOLUMN - 1;
                        a--;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a chess piece as char from the board. 
        /// Meaning of the pieces can be found in the Enum Model.Pieces
        /// </summary>
        /// <param name="row">The Row of the chess board must be between 0 and Program.MAXROW</param>
        /// <param name="column">The Column of the chess board must be between 0 and Program.MAXCOLUMN</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws Argument out of Range Exception when argument row or column are smaller than 0 or larger than Program.MAXROW/Program.MAXCOLUMN</exception>
        /// <returns>a chess piece as char from the board</returns>
        public char Get(int row, int column)
        {
            if (row < 0 || row >= Program.MAXROW)
                throw new ArgumentOutOfRangeException("row is out of Range. must be between 0 and Program.MAXROW");
            if (column < 0 || column >= Program.MAXCOLUMN)
                throw new ArgumentOutOfRangeException("column is out of Range. must be between 0 and Program.MAXCOLUMN");
            return board[row, column];
        }

        public override string ToString()
        {
            string s = "";
            int row = Program.MAXROW;
            for (int a = Program.MAXROW - 1; a >= 0; a--)
            {
                s += row-- + " ";
                for (int b = Program.MAXCOLUMN - 1; b >= 0; b--)
                {
                    s += Get(a, b);
                }
                s += "\n";
            }
            return s;
        }

        public string ToStringClean()
        {
            string s = "";
            for (int a = Program.MAXROW - 1; a >= 0; a--)
            {
                for (int b = Program.MAXCOLUMN - 1; b >= 0; b--)
                {
                    s += Get(a, b);
                }
                //s += "\n";
            }
            return s;
        }

        /// <summary>
        /// Gets the Real Saved Board how it is stored in the char array and observed by the program itself
        /// </summary>
        public string ToStringReal()
        {
            string s = "";
            int row = 0;
            for (int a = 0; a < Program.MAXROW; a++)
            {
                s += row++ + " ";
                for (int b = 0; b < Program.MAXCOLUMN; b++)
                {
                    s += board[a, b];
                }
                s += "\n";
            }
            return s;
        }

        /// <summary>
        /// Moves a chess piece from one location to another and sets the previous position to '.'
        /// </summary>
        public void Move(Move m)
        {
            char piece = Get(m.From.Row, m.From.Column);
            if ((m.To.Row == Program.MAXROW - 1 || m.To.Row == 0) && piece == 'p')
            {
                board[m.To.Row, m.To.Column] = 'q';
            }
            else if ((m.To.Row == Program.MAXROW - 1 || m.To.Row == 0) && piece == 'P')
            {
                board[m.To.Row, m.To.Column] = 'Q';
            }
            else
            {
                board[m.To.Row, m.To.Column] = board[m.From.Row, m.From.Column];
            }
            board[m.From.Row, m.From.Column] = '.';
        }

        /// <summary>
        /// Generates a list of moves that are possible for the current board and the given color
        /// </summary>
        /// <param name="Turn">Which colors turn it currently is. Only pieces of this color get checked for possible moves</param>
        /// <returns>a generated list of moves that are possible for the current board and given color</returns>
        public List<Move> GetMoveList(Colors Turn)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < Program.MAXROW; i++)
            {
                for (int j = 0; j < Program.MAXCOLUMN; j++)
                {
                    if (Get(i, j) != '.' && ColorOfPice(Get(i, j)) == Turn)
                    {
                        moves.AddRange(MoveList(new Square(i, j)));
                    }
                }
            }

            foreach (Move m in moves)
            {
                char c = char.ToLower(Get(m.To.Row, m.To.Column));
                Pieces p = (Pieces)c;
                switch (p)
                {
                    case Pieces.King:
                        m.Score = 1000000;
                        break;
                    case Pieces.Queen:
                        m.Score = 900;
                        break;
                    case Pieces.Bishop:
                    case Pieces.Knight:
                        m.Score = 300;
                        break;
                    case Pieces.Rook:
                        m.Score = 500;
                        break;
                    case Pieces.Pawn:
                        m.Score = 100;
                        break;
                }
                if ((m.To.Row == Program.MAXROW - 1 || m.To.Row == 0) && p == Pieces.Pawn)
                {
                    m.Score += 600;
                }
            }
            moves = moves.OrderByDescending(x => x.Score).ToList();
            foreach (Move m in moves)
            {
                m.Score = 0;
            }

            return moves;//.OrderBy(x => x.Score).ToList();
            //return moves;
        }


        /// <summary>
        /// Generates a list of moves that are possible for the current board and the given square
        /// </summary>
        /// <param name="square">which square should be checked for possible moves</param>
        /// <returns>a generated list of moves that are possible for the current board and given square</returns>
        private List<Move> MoveList(Square square)
        {
            char c = Get(square.Row, square.Column);
            if (c == '.')
                throw new Exception("no piece given");

            List<Move> moves = new List<Move>();

            Pieces p = (Pieces)char.ToLower(c);
            switch (p)
            {
                case Pieces.King:
                case Pieces.Queen:
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                if (dx == 0 && dy == 0)
                                    continue;
                                moves.AddRange(MoveScan(square, dx, dy, stopshort: p == Pieces.King));
                            }
                        }
                    }
                    break;
                case Pieces.Bishop:
                case Pieces.Rook:
                    {
                        int dx = 1;
                        int dy = 0;
                        for (int i = 1; i <= 4; i++)
                        {
                            moves.AddRange(MoveScan(square, dx, dy, p == Pieces.Rook, p == Pieces.Bishop));
                            SwapAndNegateSecond(ref dx, ref dy);
                        }
                        if (p == Pieces.Bishop)
                        {
                            dx = 1;
                            dy = 1;
                            for (int i = 1; i <= 4; i++)
                            {
                                moves.AddRange(MoveScan(square, dx, dy, true, false));
                                SwapAndNegateSecond(ref dx, ref dy);
                            }
                        }
                    }
                    break;
                case Pieces.Knight:
                    {
                        int dx = 1;
                        int dy = 2;
                        for (int i = 1; i <= 4; i++)
                        {
                            moves.AddRange(MoveScan(square, dx, dy, stopshort: true));
                            SwapAndNegateSecond(ref dx, ref dy);
                        }
                        dx = -1;
                        dy = 2;
                        for (int i = 1; i <= 4; i++)
                        {
                            moves.AddRange(MoveScan(square, dx, dy, stopshort: true));
                            SwapAndNegateSecond(ref dx, ref dy);
                        }
                    }
                    break;
                case Pieces.Pawn:
                    {
                        int dir = 1;
                        if (ColorOfPice(c) == Colors.BLACK)
                            dir = -1;

                        List<Move> m = MoveScan(square, -1, dir, stopshort: true);
                        if (m.Count == 1 && Get(m[0].To.Row, m[0].To.Column) != '.') //capture
                            moves.AddRange(m);
                        m = MoveScan(square, 1, dir, stopshort: true);
                        if (m.Count == 1 && Get(m[0].To.Row, m[0].To.Column) != '.') //capture
                            moves.AddRange(m);
                        moves.AddRange(MoveScan(square, 0, dir, false, true));
                    }
                    break;
            }
            return moves;
        }

        /// <summary>
        /// Generates Moves for a piece and a direction. Uses Capture and StopShort to define the distance the piece can move and wheter or not it can capture another piece in this move
        /// </summary>
        /// <param name="square">Square of the Piece whose moves should be scanned</param>
        /// <param name="dx">Row change</param>
        /// <param name="dy">Column change</param>
        /// <param name="capture">wheter the piece can capture another piece in this move</param>
        /// <param name="stopshort">wheter the piece can move a single square or multiple squares (true for single square)</param>
        /// <returns>The generated List of Moves that are possible</returns>
        private List<Move> MoveScan(Square square, int dx, int dy, bool capture = true, bool stopshort = false)
        {
            int x = square.Column;
            int y = square.Row;
            Colors c = ColorOfPice(Get(y, x));
            List<Move> moves = new List<Move>();
            do
            {
                x = x + dx;
                y = y + dy;
                if (x >= Program.MAXCOLUMN || x < 0 || y >= Program.MAXROW || y < 0)
                    break;
                char current = Get(y, x);
                if (current != '.')
                {
                    if (ColorOfPice(current) == c)
                        break;
                    if (!capture)
                        break;
                    stopshort = true;
                }
                moves.Add(new Move(square.Row, square.Column, y, x));
            } while (!stopshort);
            return moves;
        }

        /// <summary>
        /// Returns the color of the given piece
        /// </summary>
        /// <param name="c">a chess piece as char</param>
        /// <returns>the color of the given piece</returns>
        private Colors ColorOfPice(char c)
        {
            if (c >= 'A' && c <= 'Z')
                return Colors.WHITE;
            else if (c >= 'a' && c <= 'z')
                return Colors.BLACK;
            else
                return Colors.NONE;
        }

        /// <summary>
        /// Used in MoveList to swap two variables and negate the second one.
        /// </summary>
        private void SwapAndNegateSecond(ref int dx, ref int dy)
        {
            int temp = dx;
            dx = dy;
            dy = temp * -1;
        }

        public int CurrentScore(Colors Turn, Colors Won, bool draw)
        {
            if (draw)
            {
                return 0;
            }
            int scoreWhite = 0;
            int scoreBlack = 0;
            bool whiteKingAlive = false;
            bool blackKingAlive = false;
            for (int i = 0; i < Program.MAXROW; i++)
            {
                for (int j = 0; j < Program.MAXCOLUMN; j++)
                {
                    char c = Get(i, j);
                    Pieces piece = (Pieces)char.ToLower(c);
                    Colors color = ColorOfPice(c);
                    int temp = 0;
                    switch (piece)
                    {
                        case Pieces.King:
                            if (color == Colors.WHITE)
                            {
                                whiteKingAlive = true;
                            }
                            else if (color == Colors.BLACK)
                            {
                                blackKingAlive = true;
                            }
                            else
                            {
                                throw new Exception();
                            }
                            temp = 10000;
                            break;
                        case Pieces.Queen:
                            temp = 900;
                            break;
                        case Pieces.Bishop:
                            temp = 300;
                            break;
                        case Pieces.Knight:
                            temp = 300;
                            break;
                        case Pieces.Rook:
                            temp = 500;
                            break;
                        case Pieces.Pawn:
                            temp = 100;
                            break;
                    }
                    if (color == Colors.WHITE)
                        scoreWhite += temp;
                    else if(color == Colors.BLACK)
                        scoreBlack += temp;
                }
            }
            if (!blackKingAlive)
            {
                if (Turn == Colors.BLACK)
                {
                    return -10000;
                }else if(Turn == Colors.WHITE){
                    return 10000;
                }
            }
            if (!whiteKingAlive)
            {
                if (Turn == Colors.WHITE)
                {
                    return -10000;
                }
                else if (Turn == Colors.BLACK)
                {
                    return 10000;
                }
            }
            if (Turn == Colors.BLACK)
            {
                return scoreBlack - scoreWhite;
            }
            else if (Turn == Colors.WHITE)
            {
                return scoreWhite - scoreBlack;
            }
            else
            {
                throw new Exception("shouldn't happen");
            }
        }
    }
}
