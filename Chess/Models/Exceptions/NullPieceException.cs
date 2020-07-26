using System;
using System.Collections.Generic;
using System.Text;

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