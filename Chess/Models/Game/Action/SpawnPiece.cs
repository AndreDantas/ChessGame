using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Pieces;
using Chess.Models.Exceptions;
using Chess.Models.Constants;
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
        public Piece Piece;

        /// <summary>
        /// The position on the board
        /// </summary>
        public Position Position;

        public SpawnPiece(Piece piece, Position position)
        {
            this.Piece = piece.Clone();
            this.Position = position;
        }

        public void Execute(Chessboard board)
        {
            if (board == null || Piece == null)
                throw new ArgumentNullException("Board or piece can't be null");

            Tile spawnTile = board.GetTile(Position);

            if (spawnTile == null)
                throw new InvalidPositionException("Spawn's position {0} is invalid in this board");

            if (spawnTile.Piece != null)
                throw new InvalidPositionException("Can't spawn piece on occupied tile");

            spawnTile.Piece = Piece;
            Piece.CurrentPosition = Position;
        }

        public void Revert(Chessboard board)
        {
            if (board == null || Piece == null)
                throw new ArgumentNullException("Board or piece can't be null");

            Tile spawnTile = board.GetTile(Position);

            if (spawnTile == null)
                throw new InvalidPositionException("Spawn's position {0} is invalid in this board");

            spawnTile.Piece = null;
        }
    }
}