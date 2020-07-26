using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Game
{
    public struct Player : IEquatable<Player>
    {
        public string Name;
        public string Id;

        public override bool Equals(object obj)
        {
            return obj is Player player && Equals(player);
        }

        public bool Equals(Player other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            var hashCode = 1460282102;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            return hashCode;
        }

        public static bool operator ==(Player left, Player right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Player left, Player right)
        {
            return !(left == right);
        }
    }
}