using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Chess.Models.Classes
{
    public struct Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        [JsonIgnore]
        public Position Sign
        {
            get
            {
                return new Position
                {
                    x = x > 0 ? 1 : x < 0 ? -1 : 0,
                    y = y > 0 ? 1 : y < 0 ? -1 : 0,
                };
            }
        }

        /// <summary>
        /// Shorthand for (0, 1)
        /// </summary>
        public static Position Up => new Position(0, 1);

        /// <summary>
        /// Shorthand for (0,-1)
        /// </summary>
        public static Position Down => new Position(0, -1);

        /// <summary>
        /// Shorthand for (-1, 0)
        /// </summary>
        public static Position Left => new Position(-1, 0);

        /// <summary>
        /// Shorthand for (1, 0)
        /// </summary>
        public static Position Right => new Position(1, 0);

        /// <summary>
        /// Shorthand for (1, 1)
        /// </summary>
        public static Position TopRight => new Position(1, 1);

        /// <summary>
        /// Shorthand for (-1, 1)
        /// </summary>
        public static Position TopLeft => new Position(-1, 1);

        /// <summary>
        /// Shorthand for (1, -1)
        /// </summary>
        public static Position BottomRight => new Position(1, -1);

        /// <summary>
        /// Shorthand for (-1, -1)
        /// </summary>
        public static Position BottomLeft => new Position(-1, -1);

        /// <summary>
        /// Shorthand for (0, 0)
        /// </summary>
        public static Position Zero => new Position(0, 0);

        /// <summary>
        /// A list of all cardinal directions
        /// </summary>
        public static List<Position> Cardinal = new List<Position>
                {
                    Up,
                    Down,
                    Left,
                    Right
                };

        /// <summary>
        /// A list of all diagonal directions
        /// </summary>
        public static List<Position> Diagonal = new List<Position>
                {
                    TopRight,
                    TopLeft,
                    BottomRight,
                    BottomLeft
                };

        public static Position operator +(Position a, Position b)
        {
            return new Position
            {
                x = a.x + b.x,
                y = a.y + b.y
            };
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position
            {
                x = a.x - b.x,
                y = a.y - b.y
            };
        }

        public static Position operator *(Position a, Position b)
        {
            return new Position
            {
                x = a.x * b.x,
                y = a.y * b.y
            };
        }

        public static Position operator /(Position a, Position b)
        {
            return new Position
            {
                x = a.x / b.x,
                y = a.y / b.y
            };
        }

        public static Position operator *(Position a, int b)
        {
            return new Position
            {
                x = a.x * b,
                y = a.y * b
            };
        }

        public static Position operator *(int a, Position b)
        {
            return b * a;
        }

        public static Position operator /(Position a, int b)
        {
            return new Position
            {
                x = a.x / b,
                y = a.y / b
            };
        }

        public static Position operator /(int a, Position b)
        {
            return b / a;
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   x == position.x &&
                   y == position.y;
        }
    }
}