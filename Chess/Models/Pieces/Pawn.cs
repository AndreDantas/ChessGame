using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Game;
using Chess.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Pieces
{
    /// <summary>
    /// A pawn can move forward to the unoccupied square immediately in front of it on the same file,
    /// or on its first move it can advance two squares along the same file, provided both squares are unoccupied;
    /// or the pawn can capture an opponent's piece on a square diagonally in front of it on an adjacent file,
    /// by moving to that square. A pawn has two special moves: the en passant capture and promotion.
    /// </summary>
    public class Pawn : Piece
    {
        public Position MoveDirection = Position.Up;
        public override string Name => Constants.Pieces.PAWN;

        protected Pawn(Position position = default, Player player = default, int moveCount = 0) : base(position, player, moveCount)
        {
        }

        public Pawn(Position position = default, Player player = default, Position moveDirection = default, int moveCount = 0) : base(position, player, moveCount)
        {
            MoveDirection = moveDirection != Position.Zero ? moveDirection : Position.Up;
        }

        public override Piece Clone()
        {
            return new Pawn(CurrentPosition, Player, MoveDirection, MoveCount);
        }

        protected override List<Move> CalculateMoves(Chessboard Board)
        {
            var moves = new List<Move>();

            #region Forward 1

            Position pos = CurrentPosition + MoveDirection.Sign;
            Tile tile = Board.GetTile(pos);

            if (tile != null && tile.Piece == null)
                moves.Add(CreateMove(pos));

            #endregion Forward 1

            #region Forward 2

            if (MoveCount == 0 && tile != null && tile.Piece == null)
            {
                pos = CurrentPosition + MoveDirection.Sign * 2;
                tile = Board.GetTile(pos);

                if (tile != null && tile.Piece == null)
                    moves.Add(CreateMove(pos));
            }

            #endregion Forward 2

            #region Attack Left

            pos = Functions.Rotate(CurrentPosition + MoveDirection.Sign * 2, CurrentPosition + MoveDirection.Sign, 90);
            tile = Board.GetTile(pos);

            if (tile != null && tile.Piece != null && tile.Piece.Player != this.Player)
                moves.Add(CreateMove(pos, pos));

            #endregion Attack Left

            #region Attack Right

            pos = Functions.Rotate(CurrentPosition + MoveDirection.Sign * 2, CurrentPosition + MoveDirection.Sign, -90);
            tile = Board.GetTile(pos);

            if (tile != null && tile.Piece != null && tile.Piece.Player != this.Player)
                moves.Add(CreateMove(pos, pos));

            #endregion Attack Right

            //TODO: En Passant

            return moves;
        }

        public override bool Equals(object obj)
        {
            return obj is Pawn pawn &&
                   base.Equals(obj) &&
                   EqualityComparer<Position>.Default.Equals(CurrentPosition, pawn.CurrentPosition) &&
                   Player.Equals(pawn.Player) &&
                   MoveCount == pawn.MoveCount &&
                   EqualityComparer<Position>.Default.Equals(MoveDirection, pawn.MoveDirection) &&
                   Name == pawn.Name;
        }

        public override int GetHashCode()
        {
            int hashCode = 409305774;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + CurrentPosition.GetHashCode();
            hashCode = hashCode * -1521134295 + Player.GetHashCode();
            hashCode = hashCode * -1521134295 + MoveCount.GetHashCode();
            hashCode = hashCode * -1521134295 + MoveDirection.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}