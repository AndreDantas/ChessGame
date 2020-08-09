using System;

namespace Chess.Models.Exceptions
{
    public class InvalidPositionException : Exception
    {
        public InvalidPositionException()
        {
        }

        public InvalidPositionException(string message) : base(message)
        {
        }
    }
}