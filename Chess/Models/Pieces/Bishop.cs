using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Pieces
{
    /// <summary>
    /// A bishop can move any number of squares diagonally, but cannot leap over other pieces.
    /// </summary>
    public class Bishop : Piece
    {
        public override string Name => Constants.Pieces.BISHOP;

        public Bishop(Position position = default, Player player = default, int moveCount = 0) : base(position, player, moveCount)
        {
        }

        public override Piece Clone()
        {
            return new Bishop(CurrentPosition, Player, MoveCount);
        }

        protected override List<Move> CalculateMoves(Chessboard Board)
        {
            var moves = new List<Move>();

            var Directions = Position.Diagonal;

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