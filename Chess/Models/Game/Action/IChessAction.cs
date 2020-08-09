using Chess.Models.Board;

namespace Chess.Models.Game.Action
{
    public interface IChessAction
    {
        /// <summary>
        /// Action's name for serialization
        /// </summary>
        string Name { get; }

        void Execute(Chessboard board);

        void Revert(Chessboard board);
    }
}