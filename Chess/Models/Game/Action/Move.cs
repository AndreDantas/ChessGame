using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Constants;
using Chess.Models.Exceptions;
using Chess.Models.Game.Action;
using System;
using System.Collections.Generic;

namespace Chess.Models.Game
{
    /// <summary>
    /// Represents a chess movement.
    /// </summary>
    public struct Move : IChessAction
    {
        public string Name => Actions.MOVE;

        /// <summary>
        /// The start position of the movement
        /// </summary>
        public Position StartPosition;

        /// <summary>
        /// The end position of movement
        /// </summary>
        public Position EndPosition;

        /// <summary>
        /// The positions where a piece was captured
        /// </summary>
        public List<Position> Captures;

        public List<Move> ExtraMoves;

        public Move(Move other)
        {
            StartPosition = other.StartPosition;
            EndPosition = other.EndPosition;
            Captures = other.Captures;
            ExtraMoves = other.ExtraMoves;
        }

        public void Execute(Chessboard board)
        {
            if (board == null)
                throw new ArgumentNullException("Board can't be null");

            Tile startTile = board.GetTile(StartPosition);
            Tile endTile = board.GetTile(EndPosition);

            if (startTile.Piece == null)
                throw new NullPieceException("Can't start move on empty square");

            if (startTile == null)
                throw new InvalidPositionException(String.Format("Start position {0} is invalid in this board", StartPosition.ToString()));

            if (endTile == null)
                throw new InvalidPositionException(String.Format("End position {0} is invalid in this board", EndPosition.ToString()));

            if (Captures != null)
                foreach (var capture in Captures)
                {
                    Tile tile = board.GetTile(capture);

                    if (tile == null)
                        throw new InvalidPositionException(String.Format("Capture's position {0} is invalid in this board", EndPosition.ToString()));

                    if (tile.Piece == null)
                        throw new NullPieceException(String.Format("Capture's position {0} doesn't have a piece", EndPosition.ToString()));

                    board.RemovedPieces.Push(tile.Piece);
                    tile.Piece = null;
                }

            var piece = endTile.Piece = startTile.Piece;
            piece.CurrentPosition = EndPosition;
            piece.MoveCount++;
            startTile.Piece = null;

            if (ExtraMoves != null)
                foreach (var extraMove in ExtraMoves)
                {
                    extraMove.Execute(board);
                }
        }

        public void Revert(Chessboard board)
        {
            if (board == null)
                throw new ArgumentNullException("Board can't be null");

            Tile startTile = board.GetTile(StartPosition);
            Tile endTile = board.GetTile(EndPosition);

            if (endTile.Piece == null)
                throw new NullPieceException("Can't revert move on empty square");

            if (startTile == null)
                throw new InvalidPositionException(String.Format("Start position {0} is invalid in this board", StartPosition.ToString()));

            if (endTile == null)
                throw new InvalidPositionException(String.Format("End position {0} is invalid in this board", EndPosition.ToString()));

            var piece = startTile.Piece = endTile.Piece;
            piece.CurrentPosition = StartPosition;
            piece.MoveCount--;
            endTile.Piece = null;

            if (Captures != null)
                foreach (var capture in Captures)
                {
                    Tile tile = board.GetTile(capture);

                    if (tile == null)
                        throw new InvalidPositionException(String.Format("Capture's position {0} is invalid in this board", EndPosition.ToString()));

                    if (board.RemovedPieces.Count == 0)
                        throw new IndexOutOfRangeException(String.Format("Capture's position {0} doesn't have a piece", EndPosition.ToString()));

                    tile.Piece = board.RemovedPieces.Pop();
                }

            if (ExtraMoves != null)
                foreach (var extraMove in ExtraMoves)
                {
                    extraMove.Revert(board);
                }
        }
    }
}