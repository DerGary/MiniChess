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
        public Move move(List<Move> possibleMoves)
        {
            Random r = new Random();
            int move = r.Next(possibleMoves.Count);
            return possibleMoves[move];
        }
    }

}
