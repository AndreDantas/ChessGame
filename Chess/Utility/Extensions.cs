using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Utility
{
    public static class Extensions
    {
        public static bool ValidIndex<T>(this T[,] t, int x, int y)
        {
            if (t == null)
                return false;

            int lenX = t.GetLength(0);
            int lenY = t.GetLength(1);

            return (x >= 0 && x < lenX) && (y >= 0 && y < lenY);
        }

        public static T SafePeek<T>(this Stack<T> s)
        {
            if (s == null || s.Count == 0)
                return default;

            return s.Peek();
        }
    }
}