using MiniChess.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess
{
    class Program
    {
        public const int MAXTURNS = 40;
        public const int MAXROW = 6;
        public const int MAXCOLUMN = 5;
        static void Main(string[] args)
        {
            var state = new GameState();
            while (state.TurnCount < MAXTURNS)
            {
                Console.WriteLine(state.ToString());
                string s = Console.ReadLine();
                state.Move(state.CurrentMoves[int.Parse(s)]);
                if (state.Won != Colors.NONE)
                    break;
            }
            Console.WriteLine(state.Won + " has won");
            
            //Console.WriteLine(state.ToString());
            Console.ReadLine();

        }
    }
}
