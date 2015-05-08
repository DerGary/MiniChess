/* Copyright 2015 by Stefan Gerasch */

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
            Move m = StartNegaMax();
            return m;
        }
        public Move StartNegaMax() {
            List<Move> _movesTop = _state.GenerateAllLegalMoves();
            NegaMax.NegamaxRevert(_depth, _state, possibleMoves: _movesTop);
            int max = _movesTop.Max(x => x.Score);
            var move = _movesTop.First(x => x.Score == max);
            //randomness breaks the algorithm
            //int index = Program.RANDOM.Next(move.Count()); 
            //return move.ToList()[index];
            return move;
        }


        //public Move StartNegamaxParallel(List<Move> moves)
        //{
        //    List<Thread> threadList = new List<Thread>();
        //    int moveCount = moves.Count;
        //    List<List<Move>> listList = new List<List<Move>>()
        //    {
        //        new List<Move>()
        //    };
        //    for (int i = 0; i < moves.Count; i++)
        //    {
        //        listList[i % listList.Count].Add(moves[i]);
        //    }
        //    for (int i = 0; i < listList.Count; i++)
        //    {
        //        Thread t = new Thread(threadStart);
        //        t.Start(listList[i]);
        //        threadList.Add(t);
        //    }
        //    foreach (Thread t in threadList)
        //    {
        //        t.Join();
        //    }
        //    int max = moves.Max(x => x.Score);
        //    var list = moves.Where(x => x.Score == max);
        //    int index = Program.RANDOM.Next(list.Count());
        //    return list.ToList()[index];
        //}

        //private int Negamax(int depth, GameState state)
        //{
        //    if (state.Turn == Colors.NONE || depth == 0)
        //    {
        //        return state.StateScore();
        //    }
        //    int v2 = int.MinValue;

        //    List<Move> moves = state.GenerateAllLegalMoves();

        //    for (int i = 0; i < moves.Count; i++)
        //    {
        //        //GameState newState = new GameState(state);
        //        state.Move(moves[i]);
        //        int v = -(Negamax(depth - 1, new GameState(state)));
        //        state.RevertMove(moves[i]);
        //        moves[i].Score = v;
        //        if (v > v2)
        //        {
        //            v2 = v;
        //        }
        //    }

        //    return v2;
        //}

        //private void threadStart(object obj)
        //{
        //    List<Move> m = (List<Move>)obj;
        //    GameState threadState = new GameState(_state);
        //    foreach (Move item in m)
        //    {
        //        //GameState newState = new GameState(_state);
        //        threadState.Move(item);
        //        item.Score = -Negamax(_depth - 1, new GameState(threadState));
        //        threadState.RevertMove(item);
        //    }
        //}
    }
}
