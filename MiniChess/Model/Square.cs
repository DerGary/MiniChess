using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    /// <summary>
    /// Represents a single square of the chess board.
    /// </summary>
    public class Square
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        /// <summary>
        /// Initializes a new Square with row and column as properties
        /// </summary>
        /// <param name="row">the row of the chess board. row must be between 0 and Program.MAXROW</param>
        /// <param name="column">the column of the chess board. column must be between 0 and Program.MAXCOLUMN</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws an ArgumentOutOfRangeException when row or column are out of range</exception>
        public Square(int row,int column)
        {
            if (row < 0 || row > Program.MAXROW)
                throw new ArgumentOutOfRangeException("Row must be between 0 and Program.MAXROW");
            if (column < 0 || column > Program.MAXCOLUMN)
                throw new ArgumentOutOfRangeException("Column must be between 0 and Program.MAXCOLUMN");
            Row = row;
            Column = column;
        }
    }
}
