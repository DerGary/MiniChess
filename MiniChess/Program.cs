/* Copyright 2015 by Stefan Gerasch */

using MiniChess.Model;
using MiniChess.Model.Connection;
using MiniChess.Model.Enums;
using MiniChess.Model.Players;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniChess
{
    class Program
    {
        public const int MAXTURNS = 80;
        public const int MAXROW = 6;
        public const int MAXCOLUMN = 5;
        public static Random RANDOM = new Random();
        static void Main(string[] args)
        {
            //for (int j = 0; j < 1; j++)
            //{
            //    var gameCenter = new GameCenter();
            //    gameCenter.PlayGames(200, new LookaheadPlayer(3), new GreedyPlayer());
            //}

            dostuff();

            Console.WriteLine("fertig");
            Console.ReadLine();
        }
        public static void dostuff()
        {
            //Server server = new Server("193.175.31.102", 80);
            Server server = new Server("131.252.214.11", 3589);
            GameCenter center = new GameCenter();
            AlphaBetaTimedPlayer player = new AlphaBetaTimedPlayer(7.3);
            //center.PlayGameOnServer(player, server, false, 10599);
            center.PlayGameOnServer(player, server, true, startColor: Colors.WHITE); 
        }
    }
}
