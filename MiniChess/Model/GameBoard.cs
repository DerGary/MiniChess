using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess
{
    public enum Colors{
        WHITE, BLACK
    }
    public class GameBoard
    {
        private static int firstdimension = 6, seconddimension = 5;
        private char[,] board = new char[firstdimension,seconddimension];
        public int TurnCount { get; private set; }
        public Colors Turn { get; private set; }

        public GameBoard(string s = "kqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK", int turncount = 0, Colors turn = Colors.WHITE)
        {
            MakeBoard(s);
            TurnCount = turncount;
            Turn = turn;
        }

        private void MakeBoard(string p)
        {
            int a = 0, b = 0;
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i] != '\n' && p[i] != '\r')
                {
                    board[a, b] = p[i];
                    b++;
                    if (b == seconddimension)
                    {
                        b = 0;
                        a++;
                    }
                }
            }
        }
        public override string ToString()
        {
            string s = "  abcde\n\n";
            int row = firstdimension;
            for (int a = 0; a < firstdimension; a++)
            {
                s += row-- + " ";
                for (int b = 0; b < seconddimension; b++)
                {
                    s += board[a, b];
                }
                s += "\n";
            }
            return s;
        }

        public void Move(Move m)
        {
            board[firstdimension - m.To.Row, (int)m.To.Column] = board[firstdimension- m.From.Row, (int)m.From.Column];
            board[firstdimension - m.From.Row, (int)m.From.Column] = '.';
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">a1-b2</param>
        public void Move(string s)
        {
            Move(new Move((Column)s[0] - 97, int.Parse(s[1].ToString()), (Column)s[3] - 97, int.Parse(s[4].ToString())));
        }
    }
}
