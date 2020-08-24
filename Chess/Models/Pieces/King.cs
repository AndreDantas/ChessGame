using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Game;
using System.Collections.Generic;
using static Chess.Models.Board.Chessboard;

namespace Chess.Models.Pieces
{
    public class King : ChessPiece
    {
        public override string Name => Constants.ChessPieces.KING;

        public King(Position position = default, Player player = default, int moveCount = 0) : base(position, player, moveCount)
        {
        }

        public override ChessPiece Clone()
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
                TileInfo tile = Board.GetTileInfo(pos);

                if (!tile.IsValid)
                    continue;

                if (tile.hasPiece)
                {
                    if (Board.GetPiece(pos).Player != this.Player)
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