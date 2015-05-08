using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public static class NegaMax
    {
        public static int NegamaxRevert(int depth, GameState state, bool alphaBeta = false, int alpha = 0, int beta = 0, DateTime end = default(DateTime), int iteration = -1, List<Move> possibleMoves = null)
        {
            if (iteration > -1 && iteration % 20 == 0 && end <= DateTime.Now)
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
                state.Move(moves[i]);
                int v = -(NegamaxRevert(depth - 1, new GameState(state),alphaBeta, -beta, -alpha, end, iteration == -1 ? -1 : ++iteration));
                state.RevertMove(moves[i]);
                moves[i].Score = v;
                if (alphaBeta) //alpha beta should be used
                {
                    if (v >= beta)
                    {
                        return beta + 1;
                    }
                    v2 = Math.Max(v2, v);
                    alpha = Math.Max(alpha, v);
                }
                else //normal lookahead should be used
                {
                    if (v > v2)
                    {
                        v2 = v;
                    }
                }
            }
            return v2;
        }
    }
}
