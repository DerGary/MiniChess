/* Copyright 2015 by Stefan Gerasch */

using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public class AlphaBetaTimedPlayer : IPlayer
    {
        private GameState _state;
        private double _seconds;

        public AlphaBetaTimedPlayer(double seconds)
        {
            _seconds = seconds;
        }

        public Move move(GameState state)
        {
            _state = state;
            Move m = StartNegamaxIterative();
            return m;
        }

        public Move StartNegamaxIterative()
        {
            Dictionary<int, List<Move>> movesDepth = new Dictionary<int, List<Move>>();
            int i = 0;
            try
            {
                DateTime end = DateTime.Now+TimeSpan.FromSeconds(_seconds);
                GameState newState = new GameState(_state);
                while (end > DateTime.Now)
                {
                    i++;
                    if (i == 30)
                    {
                        break;
                    }
                    List<Move> temp = newState.GenerateAllLegalMoves();
                    if (temp.Count == 0)
                    {
                        break;
                    }
                    NegaMax.NegamaxRevert(i, newState,true, -1000000, 1000000, end, 0, temp);
                    movesDepth.Add(i, temp);
                }
            }
            catch (TimeoutException)
            {

            }
            //List<Move> highestDepth;
            //movesDepth.TryGetValue(movesDepth.Max(x => x.Key), out highestDepth);
            //int max = highestDepth.Max(x => x.Score);
            //var move = highestDepth.First(x => x.Score == max);
            Move move = null;
            foreach (KeyValuePair<int,List<Move>> list in movesDepth)
            {
                int highscore = list.Value.Max(x => x.Score);
                Move m = list.Value.First(x => x.Score == highscore);
                Console.WriteLine("Depth: "+list.Key +" Move: "+ m);
                if (m.Score > -100000 || list.Key == 1)
                {
                    move = m;
                }
                if (m.Score == 100000)
                {
                    return m;
                }
            }
            //Console.WriteLine(i-1);

            //randomness breaks it
            //int index = Program.RANDOM.Next(list.Count());
            //return list.ToList()[index];
            return move;
        }
    }
}
