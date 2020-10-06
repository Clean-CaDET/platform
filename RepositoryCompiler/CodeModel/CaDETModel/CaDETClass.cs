using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool IsDataClass()
        {
            double numOfAccessors = Methods.Count(m => m.IsSimpleAccessor());
            double numOfConstructors = Methods.Count(m => m.Type.Equals(CaDETMemberType.Constructor));
            double numOfObjectOverrides = CountToStringEqualsHashCode();
            //TODO: Create a more elegant solution for thresholds.
            double dataClassThreshold = 0.9;
            
            return (numOfAccessors + numOfConstructors + numOfObjectOverrides)/Methods.Count > dataClassThreshold;
        }

        private double CountToStringEqualsHashCode()
        {
            return Methods.Count(m => m.Type.Equals(CaDETMemberType.Method) && (
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