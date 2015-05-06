using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public class RandomPlayer : IPlayer
    {
        public Move move(GameState state)
        {
            List<Move> moves = state.GenerateAllLegalMoves();
            int move = Program.RANDOM.Next(moves.Count);
            return moves[move];
        }
    }
}
