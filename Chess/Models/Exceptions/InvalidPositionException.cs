using System;
using System.Collections.Generic;
using System.Text;

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