﻿using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Game;
using Chess.Models.Game.Action;
using System.Collections.Generic;
using static Chess.Models.Board.Chessboard;

namespace Chess.Models.Pieces
{
    /// <summary>
    /// The queen combines the power of a rook and bishop and can move any number of squares along a
    /// rank, file, or diagonal, but cannot leap over other pieces.
    /// </summary>
    public class Queen : ChessPiece
    {
        public override string Name => Constants.ChessPieces.QUEEN;

        public Queen(Position position = default, Player player = default, int moveCount = 0) : base(position, player, moveCount)
        {
        }

        public override ChessPiece Clone()
        {
            return new Queen(CurrentPosition, Player, MoveCount);
        }

        protected override List<Move> CalculateMoves(Chessboard Board)
        {
            var moves = new List<Move>();

            var Directions = Position.Diagonal;
            Directions.AddRange(Position.Cardinal);

            foreach (var Direction in Directions)
            {
                Position pos = CurrentPosition + Direction.Sign;
                TileInfo tile = Board.GetTileInfo(pos);

                while (tile.IsValid)
                {
                    if (tile.hasPiece)
                    {
                        var attackedPiece = Board.GetPiece(pos);
                        if (attackedPiece.Player != this.Player)
                            moves.Add(CreateMove(pos, new Capture(attackedPiece)));

                        break;
                    }
                    else
                    {
                        moves.Add(CreateMove(pos));

                        pos += Direction.Sign;
                        tile = Board.GetTileInfo(pos);
                    }
                }
            }

            return moves;
        }
    }
}