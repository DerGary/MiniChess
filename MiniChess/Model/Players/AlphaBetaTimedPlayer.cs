using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public class AlphaBetaTimedPlayer : IPlayer
    {
        private GameState _state;
        private double _seconds;

        public AlphaBetaTimedPlayer(double seconds)
        {
            _seconds = seconds;
        }

        public Move move(GameState state)
        {
            _state = state;
            Move m = StartNegamaxIterativeParallel();
            return m;
        }

        public Move StartNegamaxIterative()
        {
            Dictionary<int, List<Move>> movesDepth = new Dictionary<int, List<Move>>();
            int i = 0;
            try
            {
                DateTime end = DateTime.Now+TimeSpan.FromSeconds(_seconds);
                while (end > DateTime.Now)
                {
                    List<Move> temp = _state.GenerateAllLegalMoves();
                    Negamax(i, _state, -10000, 10000, end, 0, temp);
                    movesDepth.Add(i, temp);
                    i++;
                }
            }
            catch (TimeoutException)
            {

            }
            Console.WriteLine(i);
            List<Move> highestDepth;
            movesDepth.TryGetValue(movesDepth.Max(x => x.Key), out highestDepth);
            int max = highestDepth.Max(x => x.Score);
            var move = highestDepth.First(x => x.Score == max);
            //randomness breaks it
            //int index = Program.RANDOM.Next(list.Count());
            //return list.ToList()[index];
            return move;
        }

        public Move StartNegamaxIterativeParallel()
        {
            Dictionary<int, List<Move>> movesDepth = new Dictionary<int, List<Move>>();
            int i1 = 0;
            int i2 = 1;
            //int i3 = 2;
            //int i4 = 3;
                DateTime end = DateTime.Now + TimeSpan.FromSeconds(_seconds);
                Thread t1 = new Thread(new ThreadStart(() => {
                    try
                    {
                        while (end > DateTime.Now)
                        {
                            List<Move> temp = _state.GenerateAllLegalMoves();
                            Negamax(i1, _state, -10000, 10000, end, 0, temp);
                            movesDepth.Add(i1, temp);
                            i1 += 2;
                        }
                    }
                    catch (TimeoutException)
                    {

                    }
                }));
                Thread t2 = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        while (end > DateTime.Now)
                        {
                            List<Move> temp = _state.GenerateAllLegalMoves();
                            Negamax(i2, _state, -10000, 10000, end, 0, temp);
                            movesDepth.Add(i2, temp);
                            i2 += 2;
                        }
                    }
                    catch (TimeoutException)
                    {

                    }
                }));
                //Thread t3 = new Thread(new ThreadStart(() =>
                //{
                //    try
                //    {
                //        while (end > DateTime.Now)
                //        {
                //            List<Move> temp = _state.GenerateAllLegalMoves();
                //            Negamax(i3, _state, -10000, 10000, end, 0, temp);
                //            movesDepth.Add(i3, temp);
                //            i3 += 4;
                //        }
                //    }
                //    catch (TimeoutException)
                //    {

                //    }
                //}));
                //Thread t4 = new Thread(new ThreadStart(() =>
                //{
                //    try
                //    {
                //        while (end > DateTime.Now)
                //        {
                //            List<Move> temp = _state.GenerateAllLegalMoves();
                //            Negamax(i4, _state, -10000, 10000, end, 0, temp);
                //            movesDepth.Add(i4, temp);
                //            i4 += 4;
                //        }
                //    }
                //    catch (TimeoutException)
                //    {

                //    }
                //}));
                t1.Start();
                t2.Start();
                //t3.Start();
                //t4.Start();
                t1.Join();
                t2.Join();
                //t3.Join();
                //t4.Join();
                //while (end > DateTime.Now)
                //{
                //    Negamax(i1, _state, -10000, 10000, end, 0);
                //    movesDepth.Add(i1, _movesTop);
                //    i1+=4;
                //}

            List<Move> highestDepth;
            int maxDepth = movesDepth.Max(x => x.Key);
            Console.WriteLine(maxDepth);
            movesDepth.TryGetValue(maxDepth, out highestDepth);
            int max = highestDepth.Max(x => x.Score);
            var move = highestDepth.First(x => x.Score == max);

            return move;
        }


        private int Negamax(int depth, GameState state, int alpha, int beta, DateTime end, int iteration, List<Move> possibleMoves = null)
        {
            if (iteration % 20 == 0 && end < DateTime.Now)
            {
                throw new TimeoutException();
            }
            if (state.Turn == Colors.NONE || depth == 0)
            {
                return state.StateScore();
            }
            List<Move> moves = possibleMoves != null ? possibleMoves : state.GenerateAllLegalMoves();

            int v2 = int.MinValue;
            for (int i = 0; i < moves.Count; i++)
            {
                GameState newState = new GameState(state);
                newState.Move(moves[i]);
                int v = -(Negamax(depth - 1, newState, -beta, -alpha, end, ++iteration));
                moves[i].Score = v;
                if (v >= beta)
                {
                    return beta + 1;
                }
                v2 = Math.Max(v2, v);
                alpha = Math.Max(alpha, v);
            }
            return v2;
        }
    }
}
