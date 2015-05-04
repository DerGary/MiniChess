using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model
{
    public class TurnException : Exception
    {
        public TurnException(String s) : base(s) { }
    }
}
