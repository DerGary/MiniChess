using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public class LookaheadTimedPlayer : IPlayer
    {
        private GameState _state;
        private double _seconds;

        public LookaheadTimedPlayer(double seconds)
        {
            _seconds = seconds;
        }

        public Move move(GameState state)
        {
            _state = state;
            return StartNegaMaxIterative();
        }

        public Move StartNegaMaxIterative()
        {
            List<Move> movesLastDepth = null;
            int i = 1;
            try
            {
                DateTime end = DateTime.Now + TimeSpan.FromSeconds(_seconds);
                while (end > DateTime.Now)
                {
                    List<Move> temp = _state.GenerateAllLegalMoves();
                    NegaMax.NegamaxRevert(i, _state, end: end, iteration: 0, possibleMoves: temp);
                    movesLastDepth = temp;
                    //Console.WriteLine(i);
                    int max2 = movesLastDepth.Max(x => x.Score);
                    //Console.WriteLine(movesLastDepth.First(x => x.Score == max2));
                    i++;
                }
            }
            catch (TimeoutException)
            {

            }

            int max = movesLastDepth.Max(x => x.Score);
            var list = movesLastDepth.Where(x => x.Score == max);
            int index = Program.RANDOM.Next(list.Count());

            return list.ToList()[index];
        }

        //private int Negamax(int depth, GameState state, DateTime end, int iteration)
        //{
        //    if (iteration % 30 == 0 && end < DateTime.Now)
        //    {
        //        throw new TimeoutException();
        //    }
        //    if (state.Turn == Colors.NONE || depth == 0)
        //    {
        //        return state.StateScore();
        //    }
        //    List<Move> moves = state.GenerateAllLegalMoves();
        //    if (iteration == 0)
        //    {
        //        _movesTop = moves;
        //    }
        //    int v2 = int.MinValue;


        //    for (int i = 0; i < moves.Count; i++)
        //    {
        //        GameState newState = new GameState(state);
        //        newState.Move(moves[i]);
        //        int v = -(Negamax(depth - 1, newState, end, ++iteration));
        //        moves[i].Score = v;
        //        if (v > v2)
        //        {
        //            v2 = v;
        //        }
        //    }

        //    return v2;
        //}
    }
}
