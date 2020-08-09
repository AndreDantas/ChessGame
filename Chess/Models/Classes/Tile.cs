using Chess.Models.Pieces;
using System;
using System.Collections.Generic;

namespace Chess.Models.Classes
{
    public class Tile : IEquatable<Tile>
    {
        public Piece Piece;

        public Tile()
        {
        }

        public Tile(Piece piece)
        {
            this.Piece = piece;
        }

        public Tile(Tile other)
        {
            this.Piece = other.Piece?.Clone();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tile);
        }

        public bool Equals(Tile other)
        {
            return other != null &&
                   (EqualityComparer<Piece>.Default.Equals(Piece, other.Piece) || (this.Piece == null && other.Piece == null));
        }

        public override int GetHashCode()
        {
            return 816749037 + EqualityComparer<Piece>.Default.GetHashCode(Piece);
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