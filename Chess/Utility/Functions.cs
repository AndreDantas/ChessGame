using Chess.Models.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Utility
{
    public static class Functions
    {
        public static Position Rotate(Position point, Position center, int angle)
        {
            var angleRad = (angle) * (Math.PI / 180d); // Convert to radians

            var rotatedX = Math.Cos(angleRad) * (point.x - center.x) - Math.Sin(angleRad) * (point.y - center.y) + center.x;

            var rotatedY = Math.Sin(angleRad) * (point.x - center.x) + Math.Cos(angleRad) * (point.y - center.y) + center.y;

            return new Position
            {
                x = (int)Math.Floor(rotatedX),
                y = (int)Math.Floor(rotatedY)
            };
        }
    }
}
