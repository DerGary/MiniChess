﻿using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public class AlphaBetaPlayer : IPlayer
    {
        private int _depth;
        private GameState _state;

        public AlphaBetaPlayer(int depth)
        {
            _depth = depth;
        }

        public Move move(GameState state)
        {
            _state = state;
            //List<Move> moves = state.GenerateAllLegalMoves();
            Move m = NegaMax();
            return m;
        }
        //public Move NegaMaxParallel(List<Move> moves)
        //{
        //    List<Thread> threadList = new List<Thread>();
        //    int moveCount = moves.Count;
        //    List<List<Move>> listList = new List<List<Move>>();
        //    listList.Add(new List<Move>());
        //    listList.Add(new List<Move>());
        //    listList.Add(new List<Move>());
        //    listList.Add(new List<Move>());
        //    for (int i = 0; i < moves.Count; i++)
        //    {
        //        listList[i % 4].Add(moves[i]);
        //    }
        //    for (int i = 0; i < 4; i++)
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

        public Move NegaMax()
        {
            movesTop = _state.GenerateAllLegalMoves();
            negamax(_depth, _state, -1000000, 1000000);
            int max = movesTop.Max(x => x.Score);
            var list = movesTop.Where(x => x.Score == max);
            int index = Program.RANDOM.Next(list.Count());

            return list.ToList()[index];
        }

        Move m0;
        List<Move> movesTop;

        private int negamax(int depth, GameState state, int alpha, int beta)
        {
            if (state.Turn == Colors.NONE || depth == 0)
            {
                return state.StateScore();
            }
            List<Move> moves;
            if (depth == _depth)
            {
                moves = movesTop;
            }
            else
            {
                moves = state.GenerateAllLegalMoves();
            }
            GameState newState = new GameState(state);
            newState.Move(moves.First());
            int v2 = -(negamax(depth - 1, newState, -beta, -alpha));
            moves.First().Score = v2;
            if (v2 > beta)
            {
                return v2;
            }
            alpha = Math.Max(alpha, v2);
            for (int i = 1; i < moves.Count; i++)
            {
                newState = new GameState(state);
                newState.Move(moves[i]);
                int v = -(negamax(depth - 1, newState, -beta, -alpha));
                moves[i].Score = v;
                if (v >= beta)
                {
                    return v;
                }
                v2 = Math.Max(v2, v);
                alpha = Math.Max(alpha, v);
            }
            return v2;
        }

        //private void threadStart(object obj)
        //{
        //    List<Move> m = (List<Move>)obj;
        //    foreach (Move item in m)
        //    {
        //        GameState newState = new GameState(_state);
        //        newState.Move(item);
        //        item.Score = -negamax(_depth - 1, newState, int.MinValue, int.MaxValue);
        //    }
        //}
    }
}