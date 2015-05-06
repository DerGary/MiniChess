using MiniChess.Model.Enums;
using MiniChess.Model.Players;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    public class GameCenter
    {
        private GameState State { get; set; }
        private IPlayer WhitePlayer { get; set; }
        private IPlayer BlackPlayer { get; set; }
        public GameCenter(IPlayer whitePlayer, IPlayer blackPlayer)
        {
            WhitePlayer = whitePlayer;
            BlackPlayer = blackPlayer;
        }
        public Colors PlayGame(GameState state)
        {
            State = state;
            while (State.Turn != Colors.NONE)
            {
                if (State.Turn == Colors.WHITE)
                {
                    State.Move(WhitePlayer.move(State));
                }
                else if (State.Turn == Colors.BLACK)
                {
                    State.Move(BlackPlayer.move(State));
                }
            }
            return State.Won;
        }
        public void PlayGames(int playCount)
        {
            int white = 0;
            int black = 0;
            int draw = 0;
            DateTime total = DateTime.Now;
            for (int i = 0; i < playCount; i++)
            {
                DateTime dt = DateTime.Now;
                var won = PlayGame(new GameState());
                Console.WriteLine(won + " has won");
                if (won == Colors.WHITE)
                {
                    white++;
                }
                else if (won == Colors.BLACK)
                {
                    black++;
                }
                else
                {
                    draw++;
                }
                Console.WriteLine("Playtime: " + (dt - DateTime.Now));
            }
            Console.WriteLine("Total Playtime: " + (total - DateTime.Now));
            Console.WriteLine("White: " + white + " Black: " + black + " Draw: " + draw);
        }
    }
}
