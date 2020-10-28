using System;
using System.Runtime.Serialization;

namespace RepositoryCompiler.CodeModel.CodeParsers.CSharp
{
    [Serializable]
    internal class InappropriateMemberTypeException : Exception
    {
        public InappropriateMemberTypeException()
        {
        }

        public InappropriateMemberTypeException(string message) : base(message)
        {
        }

        public InappropriateMemberTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InappropriateMemberTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}