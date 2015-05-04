using MiniChess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    /// <summary>
    /// </summary>
    public class GameState
    {
        private char[,] board = new char[Program.MAXROW,Program.MAXCOLUMN];
        public int TurnCount { get; private set; }
        public Colors Turn { get; private set; }
        public Colors Self { get; private set; }
        public Colors Won { get; private set; }
        public List<Move> CurrentMoves { get; private set; }
        public GameState(string s = "0 W\nkqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK", int turncount = 0, Colors turn = Colors.WHITE, Colors self = Colors.WHITE)
        {
            MakeBoard(s);
            TurnCount = turncount;
            Turn = turn;
            Self = self;
            CurrentMoves = MoveListForAll();
        }

        private void MakeBoard(string p)
        {
            int a = Program.MAXROW-1, b = Program.MAXCOLUMN-1;
            string firstline = p.Substring(0, p.IndexOf('\n'));
            string[] split = firstline.Split(' ');
            TurnCount = int.Parse(split[0]);
            Turn = (Colors)split[1][0];
            string temp = p.Substring(p.IndexOf('\n')+1);

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] != '\n' && temp[i] != '\r')
                {
                    board[a, b] = temp[i];
                    b--;
                    if (b == -1)
                    {
                        b = Program.MAXCOLUMN - 1;
                        a--;
                    }
                }
            }
        }

        public override string ToString()
        {
            string s = TurnCount + " " + (char)Turn + "\n" + "  abcde\n\n";
            int row = Program.MAXROW;
            for (int a = Program.MAXROW-1; a >=0; a--)
            {
                s += row-- + " ";
                for (int b = Program.MAXCOLUMN-1; b >=0; b--)
                {
                    s += board[a, b];
                }
                s += "\n";
            }
            s += "\nPossible Moves: \n";
            for (int i = 0; i < CurrentMoves.Count; i++)
            {
                s += i + " " + CurrentMoves[i].ToString() + "\n";
            }
            return s;
        }

        public string ToStringReal()
        {
            string s = TurnCount + " " + (char)Turn + "\n" + "  01234\n\n";
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
        public void Move(Move m)
        {
            char c = board[m.To.Row, m.To.Column];
            board[m.To.Row, m.To.Column] = board[m.From.Row, m.From.Column];
            board[m.From.Row, m.From.Column] = '.';

            if (c == 'k' || c == 'K')
            {
                Won = Turn;
                if (Turn == Colors.BLACK)
                    TurnCount++;
                Turn = Colors.NONE;
            }
            else
            {
                if (Turn == Colors.WHITE)
                    Turn = Colors.BLACK;
                else
                    Turn = Colors.WHITE;
                if (Turn == Colors.WHITE)
                    TurnCount++;
                CurrentMoves = MoveListForAll();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">a1-b2</param>
        public void Move(string s)
        {
            Move(new Move(s.ToLower()));
        }

        private Colors ColorOfPice(char c){
            if(c >= 'A' && c <= 'Z')
                return Colors.WHITE;
            else 
                return Colors.BLACK;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="square">Square of the Piece whose moves should be scanned</param>
        /// <param name="dx">Row</param>
        /// <param name="dy">Column</param>
        /// <param name="capture"></param>
        /// <param name="stopshort"></param>
        /// <returns></returns>
        private List<Move> MoveScan(Square square, int dx, int dy, bool capture = true, bool stopshort = false){
            int x = square.Column, y = square.Row;
            Colors c = ColorOfPice(board[y, x]);
            if(c != Turn)
                throw new TurnException("Piece has the wrong color");
            List<Move> moves = new List<Move>();
            do
            {
                x = x + dx;
                y = y + dy;
                if (x >= Program.MAXCOLUMN || x < 0 || y >= Program.MAXROW || y < 0)
                    break;
                char current = board[y,x];
                if (current != '.')
                {
                    if (ColorOfPice(current) == c)
                        break;
                    if (!capture)
                        break;
                    stopshort = true;
                }
                moves.Add(new Move(square.Row, square.Column, y,x));
            } while (!stopshort);
            return moves;
        }

        public void SwapAndNegateSecond(ref int dx, ref int dy)
        {
            int temp = dx;
            dx = dy;
            dy = temp*-1;
        }
        private List<Move> MoveListForAll(){
            List<Move> moves = new List<Move>();
            for(int i = 0; i < Program.MAXROW; i++){
                for(int j = 0; j < Program.MAXCOLUMN; j++){
                    if(board[i,j] != '.' && ColorOfPice(board[i,j]) == Turn){
                        moves.AddRange(MoveList(new Square(i,j)));
                    }
                }
            }
            return moves;
        }
        private List<Move> MoveList(Square square)
        {
            char c = board[square.Row, square.Column];
            List<Move> moves = new List<Move>();
            Pieces p = (Pieces)c.ToString().ToLower().First();
            //int dx = 0, dy = 0;
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
                            //int temp = dx;
                            //dx = dy;
                            //dy = dx *-1;
                        }
                        if (p == Pieces.Bishop)
                        {
                            dx = 1;
                            dy = 1;
                            for (int i = 1; i <= 4; i++)
                            {
                                moves.AddRange(MoveScan(square, dx,dy,true,false));
                                SwapAndNegateSecond(ref dx, ref dy); 
                                //int temp = dx;
                                //dx = dy;
                                //dy = dx *-1;
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
                            moves.AddRange(MoveScan(square,dx,dy,stopshort:true));
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
                    int dir = 1;
                    if (ColorOfPice(c) == Colors.BLACK)
                        dir = -1;
                               
                    List<Move> m = MoveScan(square, -1, dir, stopshort:true);
                    if (m.Count == 1 && board[m[0].To.Row, m[0].To.Column] != '.') //capture
                        moves.AddRange(m);
                    m = MoveScan(square, 1, dir, stopshort:true);
                    if (m.Count == 1 && board[m[0].To.Row, m[0].To.Column] != '.') //capture
                        moves.AddRange(m);
                    moves.AddRange(MoveScan(square,0,dir,false,true));
                    break;
            }
            return moves;
        }
    }
}
