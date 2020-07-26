using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Board
{
    public class Board
    {
        private int width = 8;
        private int height = 8;
        public int Width { get => width; set => width = Math.Max(value, 1); }
        public int Height { get => height; set => height = Math.Max(value, 1); }

        public Board()
        {
        }

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}