using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public class AlphaBetaPlayer : IPlayer
    {
        private int _depth;
        private GameState _state;
        private List<Move> _movesTop;

        public AlphaBetaPlayer(int depth)
        {
            _depth = depth;
        }

        public Move move(GameState state)
        {
            _state = state;
            Move m = StartNegamax();
            return m;
        }

        private Move StartNegamax()
        {
            Negamax(_depth, _state, -100000, 100000);
            int max = _movesTop.Max(x => x.Score);
            var move = _movesTop.First(x => x.Score == max);
            //randomness breaks the algorithm
            //int index = Program.RANDOM.Next(move.Count()); 
            //return move.ToList()[index];
            return move;
        }


        private int Negamax(int depth, GameState state, int alpha, int beta)
        {
            if (state.Turn == Colors.NONE || depth == 0)
            {
                return state.StateScore();
            }
            List<Move> moves = state.GenerateAllLegalMoves();

            if (depth == _depth)
            {
                _movesTop = moves;
            }

            int v2 = int.MinValue;
            for (int i = 0; i < moves.Count; i++)
            {
                GameState newState = new GameState(state);
                newState.Move(moves[i]);
                int v = -(Negamax(depth - 1, newState, -beta, -alpha));
                moves[i].Score = v;
                if (v >= beta) //Todo: Check why not working with >=
                {
                    return beta+1;
                }
                v2 = Math.Max(v2, v);
                alpha = Math.Max(alpha, v);
            }
            return v2;
        }
    }
}
