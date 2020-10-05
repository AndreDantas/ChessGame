using Chess.Models.Classes;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Game.Action
{
    public struct Capture
    {
        public Position Position;
        public ChessPiece Piece;

        public Capture(ChessPiece piece)
        {
            this.Position = piece?.CurrentPosition ?? default;
            this.Piece = piece;
        }
    }
}