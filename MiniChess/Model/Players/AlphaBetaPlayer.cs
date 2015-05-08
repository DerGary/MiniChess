/* Copyright 2015 by Stefan Gerasch */

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
            _movesTop = _state.GenerateAllLegalMoves();
            NegaMax.NegamaxRevert(_depth, _state,true, -100000, 100000, possibleMoves:_movesTop);
            int max = _movesTop.Max(x => x.Score);
            var move = _movesTop.First(x => x.Score == max);
            //randomness breaks the algorithm
            //int index = Program.RANDOM.Next(move.Count()); 
            //return move.ToList()[index];
            return move;
        }
    }
}
