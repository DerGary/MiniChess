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

            //ReadAndTestFile();
            Console.WriteLine("fertig");
            Console.ReadLine();
        }
        public static void dostuff()
        {
            //Server server = new Server("193.175.31.102", 80);
            Server server = new Server("131.252.214.11", 3589);
            GameCenter center = new GameCenter();
            AlphaBetaTimedPlayer player = new AlphaBetaTimedPlayer(7.3);
            //center.PlayGameOnServer(player, server, false, 161);
            center.PlayGameOnServer(player, server, true); 
        }
        //public static void ReadAndTestFile()
        //{
        //    string[] lines = File.ReadAllLines("C:\\Users\\Stefan\\Desktop\\out.txt");

        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        if (i % 1000 == 0)
        //        {
        //            Console.WriteLine(i);
        //        }
        //        string[] splitstrin = lines[i].Split(' ');
        //        List<string> split = splitstrin.ToList().Where(x => !string.IsNullOrEmpty(x)).ToList();
        //        int turnCount = 39 - int.Parse(split[0]);
        //        char turn = split[1].First();
        //        string board = split[2];
        //        var state = new GameState(turnCount, (Colors)turn, board);
        //        LookaheadPlayer player = new LookaheadPlayer(3);
        //        var listMoves = player.NegaMaxParallelTest(state.GenerateAllLegalMoves(),state);
        //        int score = (int)double.Parse(split[3]);
        //        //if (state.StateScore() != score)
        //        //{
        //        //    throw new Exception();
        //        //}
        //        //var listMoves = state.GenerateAllLegalMoves();

        //        if (split.Count - 3 != listMoves.Count() * 2)
        //        {
        //            throw new Exception("movecount");
        //        }
        //        for (int j = 3; j < split.Count; j += 2)
        //        {
        //            IEnumerable<Move> list = listMoves.Where(x => x.ToStringClean() == split[j + 1]);
        //            if (list.Count() != 1)
        //            {
        //                throw new Exception("move fehlt");
        //            }
        //            if (list.First().Score != (int)double.Parse(split[j].Split('.')[0]))
        //            {
        //                throw new Exception("movescore" + list.First().Score + "!=" + (int)double.Parse(split[j]));
        //            }
        //        }
        //    }
        //}

    }
}
