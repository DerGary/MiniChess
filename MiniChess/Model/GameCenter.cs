/* Copyright 2015 by Stefan Gerasch */

using MiniChess.Model.Connection;
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

        public GameCenter()
        {
        }

        public Colors PlayGame(GameState state, IPlayer whitePlayer, IPlayer blackPlayer)
        {
            State = state;
            while (State.Turn != Colors.NONE)
            {
                if (State.Turn == Colors.WHITE)
                {
                    State.Move(whitePlayer.move(State));
                }
                else if (State.Turn == Colors.BLACK)
                {
                    State.Move(blackPlayer.move(State));
                }
            }
            return State.Won;
        }

        public void PlayGames(int playCount, IPlayer whitePlayer, IPlayer blackPlayer)
        {
            int white = 0;
            int black = 0;
            int draw = 0;
            DateTime total = DateTime.Now;
            for (int i = 0; i < playCount; i++)
            {
                DateTime dt = DateTime.Now;
                var won = PlayGame(new GameState(),whitePlayer, blackPlayer);
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

        public void PlayGameOnServer(IPlayer player, Server server, bool offer, int gameID = 0, Colors startColor = Colors.NONE)
        {
            server.StartConnection();
            server.Login();
            string gameStart = "";
            if (offer)
            {
                gameStart = server.OfferGame(startColor);
            }
            else
            {
                gameStart = server.AcceptGame(gameID, startColor);
            }
            string[] split = gameStart.Split(' ');
            Colors color = (Colors)split[1][0];
            GameState state = new GameState(self: color);
            while (state.Turn != Colors.NONE)
            {
                if (state.Turn == color)
                {
                    Move m = player.move(state);
                    state.Move(m);
                    server.SendMove(m.ToStringClean());
                    Console.WriteLine(m);
                }
                else if (state.Turn == Colors.NONE)
                {
                    break;
                }
                else
                {
                    Move m = server.Move(state);
                    if (m == null)
                    {
                        break;
                    }
                    state.Move(m);
                }
            }
            //server.GetResult();
            Console.WriteLine("Self : " + color + " Gewonnen: " + state.Won);
        }
    }
}
