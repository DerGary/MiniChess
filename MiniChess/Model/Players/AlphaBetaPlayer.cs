using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            negamax(_depth, _state, -100000, 100000);
            int max = movesTop.Max(x => x.Score);
            var move = movesTop.First(x => x.Score == max);
            //int index = Program.RANDOM.Next(move.Count());

            return move;
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
            if (v2 >= beta)
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
                if (v >= beta) //Todo: Check why not working with >=
                {
                    return beta+1;
                }
                v2 = Math.Max(v2, v);
                alpha = Math.Max(alpha, v);
            }
            return v2;
        }
    //    function negamax(node, depth, α, β, color)
    //if depth = 0 or node is a terminal node
    //    return color * the heuristic value of node
    //bestValue := -∞
    //childNodes := GenerateMoves(node)
    //childNodes := OrderMoves(childNodes)
    //foreach child in childNodes
    //    val := -negamax(child, depth - 1, -β, -α, -color)
    //    bestValue := max( bestValue, val )
    //    α := max( α, val )
    //    if α ≥ β
    //        break
    //return bestValue
        
        public int NegaMaxWiki(GameState state, int depth, int alpha, int beta){
            if(depth == 0 || state.Turn == Colors.NONE){
                return state.StateScore();
            }
            List<Move> moves = state.GenerateAllLegalMoves();
            if (depth == _depth)
            {
                movesTop = moves;
            }
            int bestValue = -10000000;
            foreach(Move m in moves){
                GameState newState = new GameState(state);
                newState.Move(m);
                int val = -NegaMaxWiki(newState, depth - 1, -beta, -alpha);
                m.Score = val;
                bestValue = Math.Max(bestValue, val);
                alpha = Math.Max(alpha, val);
                if(alpha >= beta)
                break;
            }
            return bestValue;
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
        public List<Move> NegaMaxTest(GameState state)
        {
            movesTop = state.GenerateAllLegalMoves();
            NegaMaxWiki(state, _depth, -1000, 1000);
            int max = movesTop.Max(x => x.Score);
            var list = movesTop.Where(x => x.Score == max);
            int index = Program.RANDOM.Next(list.Count());

            return movesTop;
        }
    }
}
