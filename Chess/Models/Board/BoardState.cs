using Chess.Models.Classes;
using Chess.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Chess.Models.Board
{
    public class BoardState : IEquatable<BoardState>
    {
        public Tile[,] Tiles;

        [JsonIgnore]
        public int Width { get => Tiles?.GetLength(0) ?? 0; }

        [JsonIgnore]
        public int Height { get => Tiles?.GetLength(1) ?? 0; }

        [JsonIgnore]
        public Tile this[int x, int y]
        {
            get
            {
                return Tiles.ValidIndex(x, y) ? Tiles?[x, y] : null;
            }
            set
            {
                if (Tiles.ValidIndex(x, y))
                    Tiles[x, y] = value;
            }
        }

        private BoardState()
        {
        }

        public BoardState(int Width, int Height)
        {
            Width = Math.Max(1, Width);
            Height = Math.Max(1, Height);

            Tiles = new Tile[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Tiles[i, j] = new Tile();
                }
            }
        }

        public bool ValidIndex(int x, int y) => Tiles?.ValidIndex(x, y) ?? false;

        public override bool Equals(object obj)
        {
            return Equals(obj as BoardState);
        }

        public bool Equals(BoardState other)
        {
            if (other == null)
                return false;

            if (other.Tiles == null || this.Tiles == null)
                return false;

            if (other.Tiles.GetLength(0) != this.Tiles.GetLength(0) || other.Tiles.GetLength(1) != this.Tiles.GetLength(1))

                for (int i = 0; i < this.Tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < this.Tiles.GetLength(1); j++)
                    {
                        if (!this.Tiles[i, j].Equals(other.Tiles[i, j]))
                            return false;
                    }
                }

            return true;
        }

        public override int GetHashCode()
        {
            return 1037213438 + EqualityComparer<Tile[,]>.Default.GetHashCode(Tiles);
        }

        public static bool operator ==(BoardState left, BoardState right)
        {
            return EqualityComparer<BoardState>.Default.Equals(left, right);
        }

        public static bool operator !=(BoardState left, BoardState right)
        {
            return !(left == right);
        }
    }
}