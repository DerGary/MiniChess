using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public class GreedyPlayer : IPlayer
    {
        private GameState _state;
        public Move move(GameState state)
        {
            _state = state;
            List<Move> moves = state.GenerateAllLegalMoves();

            return Greedy(moves);
        }
        public Move Greedy(List<Move> moves)
        {
            foreach (Move move in moves)
            {
                var newState = new GameState(_state);
                newState.Move(move);
                move.Score = -newState.StateScore();
            }
            int max = moves.Max(x => x.Score);
            IEnumerable<Move> iMoves = moves.Where(x => x.Score == max);
            int index = Program.RANDOM.Next(iMoves.Count());
            return iMoves.ToList()[index];
        }
    }
}
