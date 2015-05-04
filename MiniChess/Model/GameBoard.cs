using MiniChess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{

    public class GameBoard
    {
        private static int firstdimension = 6, seconddimension = 5;
        private char[,] board = new char[firstdimension,seconddimension];
        public int TurnCount { get; private set; }
        public Colors Turn { get; private set; }
        public Colors Self { get; private set; }

        public GameBoard(string s = "0 W\nkqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK", int turncount = 0, Colors turn = Colors.WHITE, Colors self = Colors.WHITE)
        {
            MakeBoard(s);
            TurnCount = turncount;
            Turn = turn;
            Self = self;
        }

        private void MakeBoard(string p)
        {
            int a = 0, b = 0;
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
            string s = TurnCount + " " + (char)Turn + "\n" + "  abcde\n\n";
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
            if (Turn != Self)
                throw new TurnException("Not your turn");

            board[firstdimension - m.To.Row, (int)m.To.Column] = board[firstdimension- m.From.Row, (int)m.From.Column];
            board[firstdimension - m.From.Row, (int)m.From.Column] = '.';
            TurnCount++;
            Turn = Self == Colors.WHITE ? Colors.BLACK : Colors.WHITE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">a1-b2</param>
        public void Move(string s)
        {
            s = s.ToLower();
            Move(new Move((Column)s[0] - 'a',
                int.Parse(s[1].ToString()),
                (Column)s[3] - 'a', 
                int.Parse(s[4].ToString())));
        }

    }
}
