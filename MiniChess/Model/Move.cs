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

        public Move(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            From = new Square(fromRow, fromColumn);
            To = new Square(toRow, toColumn);
        }
        public Move(string s)
        {
            From = new Square(int.Parse(s[1].ToString())-1, (Program.MAXCOLUMN-1)- (s[0] - 'a'));
            To = new Square(int.Parse(s[4].ToString())-1, (Program.MAXCOLUMN-1)-(s[3] - 'a'));
        }
        public override string ToString()
        {
            return (From.Row + 1) +""+ (char)(((Program.MAXCOLUMN - 1) - From.Column) + 'a') + "-" + (To.Row + 1) + (char)(((Program.MAXCOLUMN - 1) - To.Column) + 'a');
        }
    }

}
