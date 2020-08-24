using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Constants;
using Chess.Models.Exceptions;
using Chess.Models.Game.Action;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using static Chess.Models.Board.Chessboard;

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

            ChessPiece movePiece = board.GetPiece(StartPosition);
            var startTileInfo = board.GetTileInfo(StartPosition);
            var endTileInfo = board.GetTileInfo(EndPosition);

            if (movePiece == null)
                throw new NullPieceException("Can't start move on empty square");

            if (!startTileInfo.IsValid)
                throw new InvalidPositionException(String.Format("Start position {0} is invalid in this board", StartPosition.ToString()));

            if (!endTileInfo.IsValid)
                throw new InvalidPositionException(String.Format("End position {0} is invalid in this board", EndPosition.ToString()));

            if (Captures != null)
                foreach (var capture in Captures)
                {
                    TileInfo tile = board.GetTileInfo(capture);

                    if (!tile.IsValid)
                        throw new InvalidPositionException(String.Format("Capture's position {0} is invalid in this board", capture.ToString()));

                    if (!tile.hasPiece)
                        throw new NullPieceException(String.Format("Capture's position {0} doesn't have a piece", capture.ToString()));

                    board.RemovedPieces.Push(board.RemovePiece(capture));
                }

            board.MovePiece(StartPosition, EndPosition);

            movePiece.MoveCount++;

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

            ChessPiece movePiece = board.GetPiece(EndPosition);
            var startTileInfo = board.GetTileInfo(StartPosition);
            var endTileInfo = board.GetTileInfo(EndPosition);

            if (movePiece == null)
                throw new NullPieceException("Can't revert move on empty square");

            if (!startTileInfo.IsValid)
                throw new InvalidPositionException(String.Format("Start position {0} is invalid in this board", StartPosition.ToString()));

            if (!endTileInfo.IsValid)
                throw new InvalidPositionException(String.Format("End position {0} is invalid in this board", EndPosition.ToString()));

            board.MovePiece(EndPosition, StartPosition);

            movePiece.MoveCount--;

            if (Captures != null)
                foreach (var capture in Captures)
                {
                    TileInfo tile = board.GetTileInfo(capture);

                    if (!tile.IsValid)
                        throw new InvalidPositionException(String.Format("Capture's position {0} is invalid in this board", capture.ToString()));

                    if (board.RemovedPieces.Count == 0)
                        throw new IndexOutOfRangeException(String.Format("Capture's position {0} doesn't have a piece", capture.ToString()));

                    board.AddPiece(board.RemovedPieces.Pop());
                }

            if (ExtraMoves != null)
                foreach (var extraMove in ExtraMoves)
                {
                    extraMove.Revert(board);
                }
        }
    }
}