using Chess.Models.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Controllers.Configuration
{
    public class DefaultChessConfiguration : GameConfiguration
    {
        public Player White;
        public Player Black;
    }
}