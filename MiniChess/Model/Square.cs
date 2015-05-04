using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    public class Square
    {
        public int Row { get; set; }
        public int Column { get; set; }
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
