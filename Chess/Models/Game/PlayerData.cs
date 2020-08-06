using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Game
{

    public class PlayerData
    {
        public Player player;
        public Dictionary<Piece, Move> movesCache = new Dictionary<Piece, Move>();
    }
}
