/* Copyright 2015 by Stefan Gerasch */

using MiniChess.Model;
using MiniChess.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniChess.Model
{
    /// <summary>
    /// A GameState Object represents a state of a game of minichess. 
    /// It defines the current pieces on the chess board, the turncount, turn color and can be used to move pieces on the gameboard.
    /// </summary>
    public class GameState
    {
        private GameBoard _board;
        public int TurnCount { get; private set; }
        //public Colors PreviousTurn { get; private set; }
        //private Colors _turn;
        public Colors Turn { get; private set; }
        public Colors Self { get; private set; }
        public Colors Won { get; private set; }

        /// <summary>
        /// Initializes a new GameState that is used to observe the chessboard, save the current state of the board and move the chess pieces.
        /// </summary>
        /// <param name="s">string that is used to initilize a GameState object. It should look like this: 0 W\nkqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK</param>
        /// <param name="turncount">The current turncount of the game default is 0</param>
        /// <param name="turn">The color whose turn it is</param>
        /// <param name="self">the self color</param>
        public GameState(string s = "0 W\nkqbnr\nppppp\n.....\n.....\nPPPPP\nRNBQK", Colors self = Colors.WHITE)
        {
            Won = Colors.NONE;
            Self = self;

            int indexOfNewLine = s.IndexOf('\n');
            string firstline = s.Substring(0, indexOfNewLine);
            string[] split = firstline.Split(' ');
            TurnCount = int.Parse(split[0])*2;
            Turn = (Colors)split[1][0];
            _board = new GameBoard(s.Substring(indexOfNewLine + 1));
        }
        public GameState(GameState state)
        {
            Won = state.Won;
            TurnCount = state.TurnCount;
            //PreviousTurn = state.PreviousTurn;
            Turn = state.Turn;
            Self = state.Self;
            _board = new GameBoard(state._board);
        }

        public GameState(int turnCount, Colors turn, string board)
        {
            Won = Colors.NONE;
            TurnCount = turnCount;
            Turn = turn;
            _board = new GameBoard(board);
        }

        /// <summary>
        /// Moves a chess piece from one square to another. 
        /// It also sets "Won" when the enemy king was captured and increases the TurnCount after a full Turn.
        /// After the move the new CurrentMoves get generated
        /// </summary>
        /// <param name="m">The move that should be made</param>
        public void Move(Move m)
        {
            char c = char.ToLower(_board.Get(m.To.Row, m.To.Column));
            _board.Move(m);

            if ((Pieces)c == Pieces.King)
            {
                Won = Turn;
                Turn = Colors.NONE;
            }
            else if (TurnCount + 1 == Program.MAXTURNS)
            {
                Won = Colors.NONE;
                Turn = Colors.NONE;
            }
            else
            {
                if (Turn == Colors.WHITE)
                    Turn = Colors.BLACK;
                else if (Turn == Colors.BLACK)
                    Turn = Colors.WHITE;
            }
            TurnCount++;
        }

        /// <summary>
        /// Returns the Score of the current State by adding the value of each piece on the board
        /// </summary>
        public int StateScore()
        {
            if (Turn == Colors.NONE)
            {
                if (TurnCount % 2 == 0)
                {
                    return _board.CurrentScoreFast(Colors.WHITE, Won, Won == Colors.NONE);
                }
                else if (TurnCount % 2 == 1)
                {
                    return _board.CurrentScoreFast(Colors.BLACK, Won, Won == Colors.NONE);
                }
                else
                {
                    throw new Exception("shouldn't happen");
                }
            }
            else
            {
                return _board.CurrentScoreFast(Turn, Won, false);
            }
        }

        public List<Move> GenerateAllLegalMoves()
        {
            return _board.GetMoveList(Turn);
        }


        /// <summary>
        /// Can be used to make a human readable move like "a1-b2". Which means from square a1 to square b2
        /// </summary>
        /// <param name="s">a1-b2</param>
        public void Move(string s)
        {
            Move(new Move(s.ToLower()));
        }

        /// <summary>
        /// Returns a string with the current TurnCount, the color whose turn it currently ist and the current chess board.
        /// </summary>
        public override string ToString()
        {
            string s = TurnCount + " " + (char)Turn + "\n" + "  abcde\n\n";
            s += _board.ToString();
            return s;
        }

        /// <summary>
        /// Retusn a string without \n, row and column numbers or characters 
        /// </summary>
        /// <returns></returns>
        public string ToStringClean()
        {
            string s = TurnCount + " " + (char)Turn + " ";// +"\n";
            s += _board.ToStringClean();
            return s;
        }

        /// <summary>
        /// Prints how the pieces are saved
        /// </summary>
        public string ToStringReal()
        {
            string s = TurnCount + " " + (char)Turn + "\n" + "  01234\n\n";
            s += _board.ToStringReal();
            return s;
        }


        public void RevertMove(Move m)
        {
            _board.RevertMove(m);
            char c = char.ToLower(_board.Get(m.To.Row, m.To.Column));
            TurnCount--;

            if ((Pieces)c == Pieces.King)
            {
                Turn = Won;
                Won = Colors.NONE;
            }
            else if (TurnCount + 1 == Program.MAXTURNS)
            {
                Won = Colors.NONE;
                Turn = Colors.BLACK;
            }
            else
            {
                if (Turn == Colors.WHITE)
                    Turn = Colors.BLACK;
                else if (Turn == Colors.BLACK)
                    Turn = Colors.WHITE;
            }
        }
    }
}
