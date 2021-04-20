﻿using System;
using System.Runtime.Serialization;

namespace SmartTutor.ContentModel.LearningObjects.Exceptions
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
