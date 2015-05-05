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
        
        Move move(List<Move> possibleMoves);
    }

    public class HumanPlayer : IPlayer
    {
        public Move move(List<Move> possibleMoves)
        {
            int mo = 0;
            do
            {
                Console.WriteLine("Choose a move from the possible moves above:");
                string s = Console.ReadLine();
                mo = int.Parse(s);
            }
            while (mo >= possibleMoves.Count || mo < 0);
            return possibleMoves[mo];
        }
    }
    public class RandomPlayer : IPlayer
    {
        private static Random random = new Random();
        public Move move(List<Move> possibleMoves)
        {
            //Random r = new Random();
            int move = random.Next(possibleMoves.Count);
            return possibleMoves[move];
        }
    }
    public class GreedyPlayer : IPlayer
    {
        private static Random random = new Random();
        public Move move(List<Move> possibleMoves)
        {
            int max = possibleMoves.Max(x => x.Score);
            IEnumerable<Move> moves = possibleMoves.Where(x => x.Score == max);
            //Random r = new Random();
            int index = random.Next(moves.Count());
            return moves.ToList()[index];
        }
    }

}
