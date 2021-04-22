using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryCompiler.CodeModel.CaDETModel.CodeItems
{
    public class CaDETClass
    {
        public string Name { get; internal set; }
        public string FullName { get; internal set; }

        public string ContainerName
        {
            get
            {
                var nameParts = FullName.Split(".");
                return string.Join(".", nameParts, 0, nameParts.Length - 1);
            }
        }

        public string SourceCode { get; internal set; }
        public CaDETClass Parent { get; internal set; }
        public CaDETClass OuterClass { get; internal set; }
        public bool IsInnerClass => OuterClass != null;
        public List<CaDETModifier> Modifiers { get; internal set; }
        public List<CaDETMember> Members { get; internal set; }
        public List<CaDETField> Fields { get; internal set; }
        public Dictionary<CaDETMetric, double> Metrics { get; internal set; }

        public List<CaDETClass> GetFieldLinkedTypes()
        {
            return Fields.SelectMany(f => f.GetLinkedTypes()).ToList();
        }

        public List<CaDETClass> GetMethodLinkedReturnTypes()
        {
            return Members.Where(m => !(m.Type is CaDETMemberType.Constructor))
                .SelectMany(m => m.GetLinkedReturnTypes()).ToList();
        }

        public List<CaDETClass> GetMethodLinkedVariableTypes()
        {
            return Members.Where(m=> !(m.Type is CaDETMemberType.Property))
                .SelectMany(m => m.Variables)
                .SelectMany(v => v.GetLinkedTypes()).ToList();
        }

        public CaDETMember FindMember(string name)
        {
            return Members.Find(method => method.Name.Equals(name));
        }

        public CaDETMember FindMemberBySignature(string signature)
        {
            return Members.Find(method => method.Signature().Equals(signature));
        }

        public CaDETField FindField(string name)
        {
            return Fields.Find(field => field.Name.Equals(name));
        }
        public bool IsPartialClass()
        {
            return Modifiers.Any(m => m.Value == CaDETModifierValue.Partial);
        }

        public bool IsDataClass()
        {
            double numOfAccessors = Members.Count(m => m.IsFieldDefiningAccessor());
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