using System;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    /// <summary>
    /// Class <c>Access</c> represents a read/write interaction between a <c>Method</c> and a <c>Field</c>.
    /// </summary>
    public class Access
    {
        public int Method { get; }
        public int Field { get; }

        public Access(int method, int field)
        {
            Method = method;
            Field = field;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Access access) return false;
            return Method == access.Method && Field == access.Field;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Method, Field);
        }
    }
}