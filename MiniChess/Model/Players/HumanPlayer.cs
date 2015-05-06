using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public class HumanPlayer : IPlayer
    {
        public Move move(GameState state)
        {
            List<Move> moves = state.GenerateAllLegalMoves();
            int mo = 0;
            do
            {
                Console.WriteLine("Choose a move from the possible moves above:");
                string s = Console.ReadLine();
                mo = int.Parse(s);
            }
            while (mo >= moves.Count || mo < 0);
            return moves[mo];
        }
    }
}
