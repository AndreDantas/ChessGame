using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Pieces
{
    public class King : Piece
    {
        public override string Name => Constants.Pieces.KING;

        public King(Position position = default, Player player = default, int moveCount = 0) : base(position, player, moveCount)
        {
        }

        public override Piece Clone()
        {
            return new King(CurrentPosition, Player, MoveCount);
        }

        protected override List<Move> CalculateMoves(Chessboard Board)
        {
            var moves = new List<Move>();

            var Directions = Position.Cardinal;
            Directions.AddRange(Position.Diagonal);

            foreach (var Direction in Directions)
            {
                Position pos = CurrentPosition + Direction;
                Tile tile = Board.GetTile(pos);

                if (tile == null)
                    continue;

                if (tile.Piece != null)
                {
                    if (tile.Piece.Player != this.Player)
                        moves.Add(CreateMove(pos, pos));

                    continue;
                }

                moves.Add(CreateMove(pos));
            }

            //TODO: Castling

            return moves;
        }
    }
}