using Chess.Models.Classes;
using Chess.Models.Game;

namespace Chess.Models.Pieces
{
    public abstract class Piece
    {
        /// <summary>
        /// Piece's current position
        /// </summary>
        public Position CurrentPosition;

        /// <summary>
        /// Piece's player
        /// </summary>
        public Player Player;
    }
}