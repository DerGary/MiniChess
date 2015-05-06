using MiniChess.Model;
using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    /// <summary>
    /// A GameState Object represents a state of a game of minichess. 
    /// It defines the current pieces on the chess board, the turncount, turn color and can be used to move pieces on the gameboard.
    /// </summary>
    public class GameState
    {
        private GameBoard board;
        public int TurnCount { get; private set; }
        public Colors PreviousTurn { get; private set; }
        private Colors _turn;
        public Colors Turn
        {
            get { return _turn; }
            private set
            {
                PreviousTurn = _turn;
                _turn = value;
            }
        }
        public Colors Self { get; private set; }
        public Colors Won { get; private set; }
        public List<Move> CurrentMoves { get; private set; }

        /// <summary>
        /// Initializes a new GameState that is used to observe the chessboard, save the current state of the board and move the chess pieces.
        /// </summary>
        /// <param name="s">string that is used to initilize a GameState object. It should look like this: 0 W\nkqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK</param>
        /// <param name="turncount">The current turncount of the game default is 0</param>
        /// <param name="turn">The color whose turn it is</param>
        /// <param name="self">the self color</param>
        public GameState(string s = "0 W\nkqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK", Colors self = Colors.WHITE)
        {
            //s = "0 W\nkq..Q.n...ppp..P.NP..P...R.B.K";
            Self = self;

            int indexOfNewLine = s.IndexOf('\n');
            string firstline = s.Substring(0, indexOfNewLine);
            string[] split = firstline.Split(' ');
            TurnCount = int.Parse(split[0]);
            Turn = (Colors)split[1][0];
            board = new GameBoard(s.Substring(indexOfNewLine + 1));
            CurrentMoves = board.GetMoveList(Turn);
        }
        public GameState(GameState state)
        {
            TurnCount = state.TurnCount;
            Turn = state.Turn;
            Self = state.Self;
            board = new GameBoard(state.board);
            //CurrentMoves = board.GetMoveList(Turn); Todo do i need this later?
        }

        /// <summary>
        /// Returns a string with the current TurnCount, the color whose turn it currently ist and the current chess board.
        /// </summary>
        public override string ToString()
        {
            string s = TurnCount + " " + (char)Turn + "\n" + "  abcde\n\n";
            s += board.ToString();
            s += "\nPossible Moves: \n";
            for (int i = 0; i < CurrentMoves.Count; i++)
            {
                s += i + " " + CurrentMoves[i].ToString() + "\n";
            }
            return s;
        }

        /// <summary>
        /// Prints how the pieces are saved
        /// </summary>
        public string ToStringReal()
        {
            string s = TurnCount + " " + (char)Turn + "\n" + "  01234\n\n";
            s += board.ToStringReal();
            return s;
        }

        /// <summary>
        /// Moves a chess piece from one square to another. 
        /// It also sets "Won" when the enemy king was captured and increases the TurnCount after a full Turn.
        /// After the move the new CurrentMoves get generated
        /// </summary>
        /// <param name="m">The move that should be made</param>
        public void Move(Move m)
        {
            char c = char.ToLower(board.Get(m.To.Row, m.To.Column));
            board.Move(m);

            if (c == 'k')
            {
                Won = Turn;
                if (Turn == Colors.BLACK)
                    TurnCount++;
                Turn = Colors.NONE;
            }
            else if (TurnCount + 1 == Program.MAXTURNS && Turn == Colors.BLACK)
            {
                TurnCount++;
                Won = Colors.NONE;
                Turn = Colors.NONE;
            }
            else
            {
                if (Turn == Colors.WHITE)
                    Turn = Colors.BLACK;
                else if (Turn == Colors.BLACK)
                    Turn = Colors.WHITE;
                if (Turn == Colors.WHITE)
                    TurnCount++;
                CurrentMoves = board.GetMoveList(Turn);
            }
        }
        public void Greedy()
        {
            foreach (Move move in CurrentMoves)
            {
                var newState = new GameState(this);
                newState.board.Move(move);
                move.Score = newState.board.CurrentScore(newState.Turn);
            }
        }

        public int StateScore()
        {
            if (Turn == Colors.NONE)
            {
                if (PreviousTurn == Colors.WHITE)
                {
                    return board.CurrentScore(Colors.BLACK);
                }
                else if (PreviousTurn == Colors.BLACK)
                {
                    return board.CurrentScore(Colors.WHITE);
                }
                else
                {
                    throw new Exception("shouldn't happen");
                }
            }
            else
            {
                return board.CurrentScore(Turn);
            }
        }

        Move m0;
        public Move NegaMax()
        {
            List<Thread> threadList = new List<Thread>();
            int moveCount = this.CurrentMoves.Count;
            List<List<Move>> listList = new List<List<Move>>();
            listList.Add(new List<Move>());
            listList.Add(new List<Move>());
            listList.Add(new List<Move>());
            listList.Add(new List<Move>());
            for (int i = 0; i < this.CurrentMoves.Count; i++)
            {
                listList[i % 4].Add(this.CurrentMoves[i]);   
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
            //negamax(5, this, true);
            int max = CurrentMoves.Max(x => x.Score);
            var list = CurrentMoves.Where(x => x.Score == max);
            int index = Program.RANDOM.Next(list.Count());
            m0 = list.ToList()[index];
            m0.BestMove = true;
            return m0;
        }
        private int negamax(int depth, GameState state)
        {
            if (state.Turn == Colors.NONE || depth == 0)
            {
                return state.StateScore();
            }
            int v2 = int.MinValue;

            for (int i = 0; i < state.CurrentMoves.Count; i++)
            {
                GameState newState = new GameState(state);
                newState.Move(state.CurrentMoves[i]);
                int v = -(negamax(depth - 1, newState));
                state.CurrentMoves[i].Score = v;
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
                GameState newState = new GameState(this);
                newState.Move(item);
                item.Score = -negamax(3, newState);
            }
        }

        /// <summary>
        /// Can be used to make a human readable move like "a1-b2". Which means from square a1 to square b2
        /// </summary>
        /// <param name="s">a1-b2</param>
        public void Move(string s)
        {
            Move(new Move(s.ToLower()));
        }

        public string ToStringClean()
        {
            string s = TurnCount + " " + (char)Turn + " ";// +"\n";
            s += board.ToStringClean();
            s += " " + CurrentMoves.Count;
            return s;
        }
    }
}
