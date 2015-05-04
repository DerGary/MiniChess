using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    public class Square
    {
        public Column Column { get; set; }
        public int Row { get; set; }
        public Square(Column column, int row)
        {
            Column = column;
            if (row < 1 || row > 6)
                throw new ArgumentOutOfRangeException("Row must be between 1 and 6");
            Row = row;
        }
    }
}
