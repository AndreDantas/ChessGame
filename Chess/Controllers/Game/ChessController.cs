using Chess.Models.Board;

namespace Chess.Controllers.Game
{
    public abstract class ChessController
    {
        public Chessboard Board { get; protected set; }
    }
}