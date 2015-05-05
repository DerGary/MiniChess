using MiniChess.Model;
using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
            //int wonWhite = 0;
            //int wonBlack = 0;
            //int draw = 0;
            //    var state = new GameState();
            //    while (state.TurnCount < MAXTURNS)
            //    {
            //        Console.WriteLine(state.ToString());
            //        Random r = new Random();
            //        int move = r.Next(state.CurrentMoves.Count);
            //        state.Move(state.CurrentMoves[move]);
            //        if (state.Won != Colors.NONE)
            //            break;
            //    }
            //    Console.WriteLine(state.ToString());
            //    Console.WriteLine(state.Won + " has won");
            //    if (state.Won == Colors.NONE)
            //        draw++;
            //    else if (state.Won == Colors.BLACK)
            //        wonBlack++;
            //    else
            //        wonWhite++;
            //    if (draw % 100 == 0)
            //    {
            //        Console.WriteLine("draw: " + draw + " white: " + wonWhite + " black: " + wonBlack);
            //        Console.ReadKey();
            //    }
            //Console.WriteLine(state.ToString());
            while(true){

            var gameCenter = new GameCenter(new GameState(), new HumanPlayer(), new RandomPlayer());
            Console.WriteLine(gameCenter.PlayGame());
            }
            Console.ReadLine();
        }

    }
}
