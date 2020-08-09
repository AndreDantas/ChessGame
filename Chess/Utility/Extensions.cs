using System;
using System.Collections.Generic;

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

        /// <summary>
        /// If the string is Empty or Null, returns the replacement string
        /// </summary>
        /// <param name="s"> </param>
        /// <param name="replacement"> </param>
        /// <returns> </returns>
        public static string IfEmpty(this string s, string replacement)
        {
            return string.IsNullOrEmpty(s) ? replacement : s;
        }

        public static T Search<T>(this List<T> l, Func<T, bool> searchFunction)
        {
            if (l == null || l.Count == 0)
                return default(T);

            for (int i = 0; i < l.Count; i++)
            {
                if (searchFunction(l[i]))
                    return l[i];
            }

            return default(T);
        }
    }
}