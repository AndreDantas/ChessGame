﻿using Chess.Models.Classes;
using Chess.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        public BoardState(BoardState other)
        {
            if (other == null || other.Tiles == null)
                return;

            Tiles = new Tile[other.Width, other.Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    this[i, j] = new Tile(other[i, j]);
                }
            }
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
                    Tiles[i, j] = new Tile(new Position(i, j));
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
            if (other == null || other.Tiles == null || this.Tiles == null)
                return false;

            if (other.Width != this.Width || other.Height != this.Height)
                return false;

            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Height; j++)
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