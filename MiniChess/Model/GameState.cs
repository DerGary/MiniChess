using MiniChess.Model;
using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Colors Turn { get; private set; }
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
        public GameState(string s = "0 W\nkqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK", int turncount = 0, Colors turn = Colors.WHITE, Colors self = Colors.WHITE)
        {
            TurnCount = turncount;
            Turn = turn;
            Self = self;

            int indexOfNewLine = s.IndexOf('\n');
            string firstline = s.Substring(0, indexOfNewLine);
            string[] split = firstline.Split(' ');
            TurnCount = int.Parse(split[0]);
            Turn = (Colors)split[1][0];
            board = new GameBoard(s.Substring(indexOfNewLine + 1));
            CurrentMoves = board.GetMoveList(Turn);
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
            else if(TurnCount+1 == Program.MAXTURNS && Turn == Colors.BLACK)
            {
                TurnCount++;
                Won = Colors.NONE;
                Turn = Colors.NONE;
            }
            else
            {
                if (Turn == Colors.WHITE)
                    Turn = Colors.BLACK;
                else
                    Turn = Colors.WHITE;
                if (Turn == Colors.WHITE)
                    TurnCount++;
                CurrentMoves = board.GetMoveList(Turn);
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
    }
}
