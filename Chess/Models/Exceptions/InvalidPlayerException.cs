using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Chess.Models.Exceptions
{
    public class InvalidPlayerException : Exception
    {
        public InvalidPlayerException()
        {
        }

        public InvalidPlayerException(string message) : base(message)
        {
        }

        public InvalidPlayerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidPlayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}