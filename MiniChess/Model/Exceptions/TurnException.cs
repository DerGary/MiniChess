using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model.Exceptions
{
    public class TurnException : Exception
    {
        public TurnException() : base() { }
        public TurnException(String message) : base(message) { }
        public TurnException(String message, Exception innerException) : base(message, innerException) { }
    }
}
