using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public class LookaheadPlayer : IPlayer
    {
        private int _depth;
        private GameState _state;

        public LookaheadPlayer(int depth)
        {
            _depth = depth;
        }

        public Move move(GameState state)
        {
            _state = state;
            List<Move> moves = state.GenerateAllLegalMoves();
            Move m = StartNegamaxParallel(moves);
            return m;
        }

        public Move StartNegamaxParallel(List<Move> moves)
        {
            List<Thread> threadList = new List<Thread>();
            int moveCount = moves.Count;
            List<List<Move>> listList = new List<List<Move>>()
            {
                new List<Move>(),new List<Move>(),new List<Move>(),new List<Move>()
            };
            for (int i = 0; i < moves.Count; i++)
            {
                listList[i % 4].Add(moves[i]);
            }
            for (int i = 0; i < 4; i++)
            {
                Thread t = new Thread(threadStart);
                t.Start(listList[i]);
                threadList.Add(t);
            }
            foreach (Thread t in threadList)
            {
                t.Join();
            }
            int max = moves.Max(x => x.Score);
            var list = moves.Where(x => x.Score == max);
            int index = Program.RANDOM.Next(list.Count());
            return list.ToList()[index];
        }

        private int Negamax(int depth, GameState state)
        {
            if (state.Turn == Colors.NONE || depth == 0)
            {
                return state.StateScore();
            }
            int v2 = int.MinValue;

            List<Move> moves = state.GenerateAllLegalMoves();

            for (int i = 0; i < moves.Count; i++)
            {
                GameState newState = new GameState(state);
                newState.Move(moves[i]);
                int v = -(Negamax(depth - 1, newState));
                moves[i].Score = v;
                if (v > v2)
                {
                    v2 = v;
                }
            }

            return v2;
        }

        private void threadStart(object obj)
        {
            List<Move> m = (List<Move>)obj;
            foreach (Move item in m)
            {
                GameState newState = new GameState(_state);
                newState.Move(item);
                item.Score = -Negamax(_depth - 1, newState);
            }
        }

        public List<Move> NegaMaxParallelTest(List<Move> moves, GameState state)
        {
            _state = state;
            List<Thread> threadList = new List<Thread>();
            int moveCount = moves.Count;
            List<List<Move>> listList = new List<List<Move>>();
            listList.Add(new List<Move>());
            listList.Add(new List<Move>());
            listList.Add(new List<Move>());
            listList.Add(new List<Move>());
            listList.Add(new List<Move>());
            listList.Add(new List<Move>());
            for (int i = 0; i < moves.Count; i++)
            {
                listList[i % listList.Count].Add(moves[i]);
            }
            for (int i = 0; i < listList.Count; i++)
            {
                Thread t = new Thread(threadStart);
                t.Start(listList[i]);
                threadList.Add(t);
            }
            foreach (Thread t in threadList)
            {
                t.Join();
            }
            return moves;
        }
    }
}
