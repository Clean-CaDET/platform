using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETClass
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string SourceCode { get; set; }
        public List<CaDETMember> Methods { get; set; }
        public List<CaDETMember> Fields { get; set; }
        public CaDETClassMetrics Metrics { get; set; }

        public override bool Equals(object? other)
        {
            return other is CaDETClass otherClass && FullName.Equals(otherClass.FullName);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return FullName.ToString();
        }
    }
}