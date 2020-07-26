using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models.Exceptions
{
    public class InvalidActionException : Exception
    {
        public InvalidActionException()
        {
        }

        public InvalidActionException(string message) : base(message)
        {
        }

        public InvalidActionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}