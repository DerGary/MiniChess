using MiniChess.Model;
using MiniChess.Model.Enums;
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
                Random r = new Random();
                int move = r.Next(state.CurrentMoves.Count);
                state.Move(state.CurrentMoves[move]);
                if (state.Won != Colors.NONE)
                    break;
            }
            Console.WriteLine(state.ToString());
            Console.WriteLine(state.Won + " has won");
            
            //Console.WriteLine(state.ToString());
            Console.ReadLine();

        }
    }
}
