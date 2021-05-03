using System;
using System.Runtime.Serialization;

namespace CodeModel.CodeParsers.CSharp.Exceptions
{
    [Serializable]
    public class NonUniqueFullNameException : Exception
    {
        public NonUniqueFullNameException()
        {
        }

        public NonUniqueFullNameException(string message) : base(message)
        {
        }

        public NonUniqueFullNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NonUniqueFullNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}