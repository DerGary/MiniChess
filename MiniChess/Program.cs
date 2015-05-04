using MiniChess.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess
{
    class Program
    {
        static void Main(string[] args)
        {
            var state = new GameBoard();

            Console.WriteLine(state.ToString());
            //state.Move(new Move(Column.a,1,Column.d,4));
            string s = Console.ReadLine();
            state.Move(s);
            Console.WriteLine(state.ToString());
            s = Console.ReadLine();

        }
    }
}
