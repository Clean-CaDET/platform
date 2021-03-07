using System;
using System.Runtime.Serialization;

namespace RepositoryCompiler.CodeModel.CodeParsers.CSharp
{
    [Serializable]
    internal class PartialIsNotSupportedException : Exception
    {
        public PartialIsNotSupportedException()
        {
        }

        public PartialIsNotSupportedException(string message) : base(message)
        {
        }

        public PartialIsNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PartialIsNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}