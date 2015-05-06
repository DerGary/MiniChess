using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    /// <summary>
    /// Represents a move from a square to another square
    /// </summary>
    public class Move
    {
        public Square From { get; private set; }
        public Square To { get; private set; }
        public int Score { get; set; }

        /// <summary>
        /// Initializes a move from a given row and column to a given row and column
        /// </summary>
        /// <param name="fromRow">the row of the square where the move starts</param>
        /// <param name="fromColumn">the column of the square where the move starts</param>
        /// <param name="toRow">the row of the square where the move ends</param>
        /// <param name="toColumn">the column of the square where the move ends</param>
        public Move(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            From = new Square(fromRow, fromColumn);
            To = new Square(toRow, toColumn);
        }

        /// <summary>
        /// Initializes a move from a string
        /// </summary>
        /// <param param name="s">is used to initialize the move. Should look like: "a1-b2"</param>
        public Move(string s)
        {
            From = new Square(int.Parse(s[1].ToString())-1, (Program.MAXCOLUMN-1)- (s[0] - 'a'));
            To = new Square(int.Parse(s[4].ToString())-1, (Program.MAXCOLUMN-1)-(s[3] - 'a'));
        }

        public override string ToString()
        {
            return (From.Row + 1) +""+ (char)(((Program.MAXCOLUMN - 1) - From.Column) + 'a') + "-" + (To.Row + 1) + (char)(((Program.MAXCOLUMN - 1) - To.Column) + 'a') + " Score: "+ Score;
        }
    }

}
