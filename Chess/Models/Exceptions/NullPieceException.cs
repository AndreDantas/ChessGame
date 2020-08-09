using System;

namespace Chess.Models.Exceptions
{
    public class NullPieceException : Exception
    {
        public NullPieceException()
        {
        }

        public NullPieceException(string message) : base(message)
        {
        }
    }
}