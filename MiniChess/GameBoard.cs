using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess
{
    public class GameBoard
    {
        private static int firstdimension = 6, seconddimension = 5;
        private char[,] board = new char[firstdimension,seconddimension];
        public int TurnCount { get; private set; }


        public GameBoard()
        {
            MakeBoard("kqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK");
        }

        public GameBoard(string s)
        {
            MakeBoard(s);
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
                    if (b == 5)
                    {
                        b = 0;
                        a++;
                    }
                }
            }
        }
        public string ToString()
        {
            string s = "";
            for (int a = 0; a < firstdimension; a++)
            {
                for (int b = 0; b < seconddimension; b++)
                {
                    s += board[a, b];
                }
                s += "\n";
            }
            return s;
        }
    }
}
