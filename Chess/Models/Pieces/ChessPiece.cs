using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Game;
using System;
using System.Collections.Generic;

namespace Chess.Models.Pieces
{
    public abstract class ChessPiece : Piece
    {
        /// <summary>
        /// Piece's name
        /// </summary>
        /// <inheritdoc />
        public abstract string Name { get; }

        private int moveCount;

        /// <summary>
        /// The number of moves this piece made
        /// </summary>
        public int MoveCount { get => moveCount; set => moveCount = Math.Max(value, 0); }

        protected ChessPiece(Position position = default, Player player = default, int moveCount = 0)
        {
            CurrentPosition = position;
            Player = player;
            MoveCount = moveCount;
        }

        /// <summary>
        /// Internal function to calculate all possible moves of this piece. Doesn't remove moves
        /// that leave the King in Check.
        /// </summary>
        /// <param name="Board"> </param>
        /// <returns> </returns>
        /// <inheritdoc />
        protected abstract List<Move> CalculateMoves(Chessboard Board);

        /// <summary>
        /// Creates a copy of this piece.
        /// </summary>
        /// <inheritdoc />
        /// <returns> </returns>
        public abstract ChessPiece Clone();

        /// <summary>
        /// Returns all available moves from this piece
        /// </summary>
        /// <returns> </returns>
        public virtual List<Move> GetMoves(Chessboard Board)
        {
            if (Board == null || Player == default)
                return new List<Move>();

            return CalculateMoves(Board);
        }

        /// <summary>
        /// Helper function to create a Move
        /// </summary>
        /// <returns> </returns>
        public virtual Move CreateMove(Position endPosition, List<Position> captures = null, List<Move> extraMoves = null)
        {
            return new Move
            {
                StartPosition = this.CurrentPosition,
                EndPosition = endPosition,
                Captures = captures ?? new List<Position>(),
                ExtraMoves = extraMoves ?? new List<Move>()
            };
        }

        /// <summary>
        /// Helper function to create a Move
        /// </summary>
        /// <returns> </returns>
        public virtual Move CreateMove(Position endPosition, Position capture, List<Move> extraMoves = null)
        {
            return CreateMove(endPosition, new List<Position> { capture });
        }

        /// <summary>
        /// Helper function to create a Move
        /// </summary>
        /// <returns> </returns>
        public virtual Move CreateMove(Position endPosition, Position capture, Move extraMove)
        {
            return CreateMove(endPosition, new List<Position> { capture }, new List<Move> { extraMove });
        }

        public override bool Equals(object obj)
        {
            return obj is ChessPiece piece &&
                   Name == piece.Name &&
                   EqualityComparer<Position>.Default.Equals(CurrentPosition, piece.CurrentPosition) &&
                   Player.Equals(piece.Player) &&
                   MoveCount == piece.MoveCount;
        }

        public override int GetHashCode()
        {
            int hashCode = -1300823941;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + CurrentPosition.GetHashCode();
            hashCode = hashCode * -1521134295 + Player.GetHashCode();
            hashCode = hashCode * -1521134295 + MoveCount.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{Name} {CurrentPosition}";
        }
    }
}