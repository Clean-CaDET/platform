using System;
using System.Runtime.Serialization;

namespace CodeModel.CodeParsers.CSharp.Exceptions
{
    [Serializable]
    internal class StructIsNotSupportedException : Exception
    {
        public StructIsNotSupportedException()
        {
        }

        public StructIsNotSupportedException(string message) : base(message)
        {
        }

        public StructIsNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StructIsNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}