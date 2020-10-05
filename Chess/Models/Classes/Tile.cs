using Chess.Models.Pieces;
using System;
using System.Collections.Generic;

namespace Chess.Models.Classes
{
    public class Tile : IEquatable<Tile>
    {
        public ChessPiece Piece;
        public Position Position;

        public Tile(Position position)
        {
            this.Position = position;
        }

        public Tile(Position position, ChessPiece piece) : this(position)
        {
            this.Piece = piece;
        }

        public Tile(Tile other) : this(other?.Position ?? default, other.Piece?.Clone())
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tile);
        }

        public bool Equals(Tile other)
        {
            return other != null &&
                   (EqualityComparer<ChessPiece>.Default.Equals(Piece, other.Piece) || (this.Piece == null && other.Piece == null)) && Position == other.Position;
        }

        public override int GetHashCode()
        {
            return 816749037 + EqualityComparer<ChessPiece>.Default.GetHashCode(Piece);
        }

        public static bool operator ==(Tile left, Tile right)
        {
            return EqualityComparer<Tile>.Default.Equals(left, right);
        }

        public static bool operator !=(Tile left, Tile right)
        {
            return !(left == right);
        }
    }
}