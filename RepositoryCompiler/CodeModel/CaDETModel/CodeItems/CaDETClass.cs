using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryCompiler.CodeModel.CaDETModel.Metrics;

namespace RepositoryCompiler.CodeModel.CaDETModel.CodeItems
{
    public class CaDETClass
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string SourceCode { get; set; }
        public CaDETClass Parent { get; set; }
        public List<CaDETModifier> Modifiers { get; set; }
        public List<CaDETMember> Members { get; set; }
        public List<CaDETField> Fields { get; set; }
        public CaDETClassMetrics Metrics { get; set; }

        public CaDETMember FindMember(string name)
        {
            return Members.Find(method => method.Name.Equals(name));
        }

        public CaDETField FindField(string name)
        {
            return Fields.Find(field => field.Name.Equals(name));
        }

        public bool IsDataClass()
        {
            double numOfAccessors = Members.Count(m => m.IsSimpleAccessor());
            double numOfConstructors = Members.Count(m => m.Type.Equals(CaDETMemberType.Constructor));
            double numOfObjectOverrides = CountToStringEqualsHashCode();
            //TODO: Create a more elegant solution for thresholds.
            double dataClassThreshold = 0.9;

            return (numOfAccessors + numOfConstructors + numOfObjectOverrides) / Members.Count > dataClassThreshold;
        }

        private double CountToStringEqualsHashCode()
        {
            return Members.Count(m => m.Type.Equals(CaDETMemberType.Method) && (
                m.Name.Contains("tostring", StringComparison.CurrentCultureIgnoreCase) ||
                m.Name.Contains("equals", StringComparison.CurrentCultureIgnoreCase) ||
                m.Name.Contains("hashcode", StringComparison.CurrentCultureIgnoreCase)));
        }

        public override bool Equals(object other)
        {
            return other is CaDETClass otherClass && FullName.Equals(otherClass.FullName);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}