using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess
{
    public class Move
    {
        public Square From { get; set; }
        public Square To { get; set; }

        public Move(Column fromColumn, int fromRow, Column toColumn, int toRow)
        {
            From = new Square(fromColumn, fromRow);
            To = new Square(toColumn, toRow);
        }
    }

}
