using MiniChess.Model.Enums;
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
        public GameCenter(GameState state, IPlayer whitePlayer, IPlayer blackPlayer)
        {
            State = state;
            WhitePlayer = whitePlayer;
            BlackPlayer = blackPlayer;
        }
        public Colors PlayGame()
        {
            //string s = "";
            while (State.Turn != Colors.NONE)
            {
            //Console.WriteLine(State);

                //s+=State.ToStringClean() +"\n";
                if (State.Turn == Colors.WHITE)
                {
                    State.Move(WhitePlayer.move(State));
                }
                else if (State.Turn == Colors.BLACK)
                {
                    State.Move(BlackPlayer.move(State));
                }
                //Console.WriteLine(State.ToString());
                //State.ToStringClean();
                //Console.ReadKey();
            }
            //Console.WriteLine(State);
            //File.AppendAllText("C:\\Users\\Stefan\\Documents\\text2.txt", s);
            return State.Won;
        }
    }
}
