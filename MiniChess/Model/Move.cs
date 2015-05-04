using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    public class Move
    {
        public Square From { get; private set; }
        public Square To { get; private set; }

        public Move(Column fromColumn, int fromRow, Column toColumn, int toRow)
        {
            From = new Square(fromColumn, fromRow);
            To = new Square(toColumn, toRow);
        }
    }

}
