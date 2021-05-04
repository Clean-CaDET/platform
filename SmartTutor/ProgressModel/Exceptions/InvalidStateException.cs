using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SmartTutor.ProgressModel.Exceptions
{
    [Serializable]
    internal class InvalidStateException : Exception
    {
        public InvalidStateException()
        {
        }

        public InvalidStateException(string message) : base(message)
        {
        }

        public InvalidStateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidStateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}