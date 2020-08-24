using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Constants;
using Chess.Models.Pieces;
using System;

namespace Chess.Models.Game.Action
{
    /// <summary>
    /// Chess' action that spawns a piece on a empty square
    /// </summary>
    public struct SpawnPiece : IChessAction
    {
        /// <summary>
        /// Action's name for serialization
        /// </summary>
        public string Name => Actions.SPAWN_PIECE;

        /// <summary>
        /// The piece to spawn
        /// </summary>
        public ChessPiece Piece;

        /// <summary>
        /// The position on the board
        /// </summary>
        public Position Position;

        public SpawnPiece(ChessPiece piece, Position position)
        {
            this.Piece = piece.Clone();
            this.Position = position;
        }

        public void Execute(Chessboard board)
        {
            if (board == null || Piece == null)
                throw new ArgumentNullException("Board or piece can't be null");

            Piece.CurrentPosition = Position;

            board.AddPiece(Piece);
        }

        public void Revert(Chessboard board)
        {
            if (board == null || Piece == null)
                throw new ArgumentNullException("Board or piece can't be null");

            board.RemovePiece(Position);
        }
    }
}