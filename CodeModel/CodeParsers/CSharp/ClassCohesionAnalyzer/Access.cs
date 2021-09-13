using System;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    /// <summary>
    /// Class <c>Access</c> represents a read/write interaction between a <c>Method</c> and a <c>DataMember</c>.
    /// </summary>
    public class Access
    {
        public int Method { get; }
        public int DataMember { get; }

        public Access(int method, int dataMember)
        {
            Method = method;
            DataMember = dataMember;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Access access) return false;
            return Method == access.Method && DataMember == access.DataMember;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Method, DataMember);
        }
    }
}