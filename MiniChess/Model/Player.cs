using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    
    public interface IPlayer
    {
        
        Move move(GameState state);
    }

    public class HumanPlayer : IPlayer
    {
        public Move move(GameState state)
        {
            int mo = 0;
            do
            {
                Console.WriteLine("Choose a move from the possible moves above:");
                string s = Console.ReadLine();
                mo = int.Parse(s);
            }
            while (mo >= state.CurrentMoves.Count || mo < 0);
            return state.CurrentMoves[mo];
        }
    }
    public class RandomPlayer : IPlayer
    {
        private static Random random = new Random();
        public Move move(GameState state)
        {
            //Random r = new Random();
            //Console.Write(state);
            int move = random.Next(state.CurrentMoves.Count);
            return state.CurrentMoves[move];
        }
    }
    public class GreedyPlayer : IPlayer
    {
        private static Random random = new Random();
        public Move move(GameState state)
        {
            state.Greedy();
            //Console.Write(state);
            //Console.ReadKey();
            int max = state.CurrentMoves.Max(x => x.Score);
            IEnumerable<Move> moves = state.CurrentMoves.Where(x => x.Score == max);
            //Random r = new Random();
            int index = random.Next(moves.Count());
            return moves.ToList()[index];
        }
    }
    public class LookaheadPlayer : IPlayer
    {
        public Move move(GameState state)
        {
            Move m = state.NegaMax();
            return m;
        }
    }

}
