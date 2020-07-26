using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Game;
using System.Collections.Generic;

namespace Chess.Models.Pieces
{
    /// <summary>
    /// A rook can move any number of squares along a rank or file, but cannot leap over other pieces.
    /// Along with the king, a rook is involved during the king's castling move.
    /// </summary>
    public class Rook : Piece
    {
        public override string Name => Constants.Pieces.ROOK;

        public Rook(Position position = default, Player player = default, int moveCount = 0) : base(position, player, moveCount)
        {
        }

        public override Piece Clone()
        {
            return new Rook(CurrentPosition, Player, MoveCount);
        }

        protected override List<Move> CalculateMoves(Chessboard Board)
        {
            var moves = new List<Move>();

            var Directions = Position.Cardinal;

            foreach (var Direction in Directions)
            {
                Position pos = CurrentPosition + Direction.Sign;
                Tile tile = Board.GetTile(pos);

                while (tile != null)
                {
                    if (tile.Piece != null)
                    {
                        if (tile.Piece.Player != this.Player)
                            moves.Add(CreateMove(pos, pos));

                        break;
                    }
                    else
                    {
                        moves.Add(CreateMove(pos));

                        pos += Direction.Sign;
                        tile = Board.GetTile(pos);
                    }
                }
            }

            return moves;
        }
    }
}