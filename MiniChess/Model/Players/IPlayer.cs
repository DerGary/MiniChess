using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniChess.Model.Players
{
    public interface IPlayer
    {
        Move move(GameState state);
    }
}
