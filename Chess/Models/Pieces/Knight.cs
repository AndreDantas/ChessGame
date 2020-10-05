using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Game;
using Chess.Models.Game.Action;
using System.Collections.Generic;
using static Chess.Models.Board.Chessboard;

namespace Chess.Models.Pieces
{
    /// <summary>
    /// A knight moves to any of the closest squares that are not on the same rank, file, or
    /// diagonal. (Thus the move forms an "L"-shape: two squares vertically and one square
    /// horizontally, or two squares horizontally and one square vertically.) The knight is the only
    /// piece that can leap over other pieces.
    /// </summary>
    public class Knight : ChessPiece
    {
        public override string Name => Constants.ChessPieces.KNIGHT;

        private List<Position> KnightPositions => new List<Position>
        {
            new Position(-1,2),
            new Position(1,2),
            new Position(2,1),
            new Position(2,-1),
            new Position(-1,-2),
            new Position(1,-2),
            new Position(-2,-1),
            new Position(-2, 1)
        };

        public Knight(Position position = default, Player player = default, int moveCount = 0) : base(position, player, moveCount)
        {
        }

        public override ChessPiece Clone()
        {
            return new Knight(CurrentPosition, Player, MoveCount);
        }

        protected override List<Move> CalculateMoves(Chessboard Board)
        {
            var moves = new List<Move>();

            foreach (var Position in KnightPositions)
            {
                Position pos = CurrentPosition + Position;
                TileInfo tile = Board.GetTileInfo(pos);

                if (!tile.IsValid)
                    continue;

                if (tile.hasPiece)
                {
                    var attackedPiece = Board.GetPiece(pos);
                    if (attackedPiece.Player != this.Player)
                        moves.Add(CreateMove(pos, new Capture(attackedPiece)));

                    continue;
                }

                moves.Add(CreateMove(pos));
            }

            return moves;
        }
    }
}