using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel.CodeParsers.CSharp.ICMBC
{
    public class Edge
    {
        public int Method { get; }
        public int Field { get; }

        public Edge(int method, int field)
        {
            Method = method;
            Field = field;
        }

        public override bool Equals(object obj)
        {
            var edge = obj as Edge;
            if (edge == null) return false;
            return Method == edge.Method && Field == edge.Field;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Method, Field);
        }
    }
}