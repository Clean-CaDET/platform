using System;

namespace CodeModel.CodeParsers.CSharp.Exceptions
{
    public class ClassWithoutElementsException : Exception
    {
        public ClassWithoutElementsException(string message) : base(message)
        {
        }
    }
}