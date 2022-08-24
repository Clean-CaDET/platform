using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.CaDETModel.CodeItems
{
    public class CaDETClass
    {
        public string Name { get; }
        public string FullName { get; }

        public string ContainerName
        {
            get
            {
                var nameParts = FullName.Split(".");
                return string.Join(".", nameParts, 0, nameParts.Length - 1);
            }
        }

        public string SourceCode { get; }
        public CaDETClass Parent { get; internal set; }
        public CaDETClass OuterClass { get; internal set; }
        public ISet<CaDETClass> InnerClasses { get; }
        public bool IsInnerClass => OuterClass != null;
        public List<CaDETModifier> Modifiers { get; internal set; }
        public List<CaDETMember> Members { get; internal set; }
        public List<CaDETField> Fields { get; internal set; }
        public Dictionary<CaDETMetric, double> Metrics { get; set; }
        public List<CaDETClass> Subclasses { get; set; }

        public CaDETClass(string name)
        {
            Name = name;
            InnerClasses = new HashSet<CaDETClass>();
        }

        public CaDETClass(string name, string fullName, string sourceCode): this(name)
        {
            FullName = fullName;
            SourceCode = sourceCode;
        }

        public List<CaDETClass> GetFieldLinkedTypes()
        {
            var cadetClasses = Fields.SelectMany(f => f.GetLinkedTypes()).ToList();
            RemoveThisClassFromList(cadetClasses);
            return cadetClasses;
        }

        public List<CaDETClass> GetMethodLinkedReturnTypes()
        {
            var cadetClasses = Members.Where(m => m.Type is not CaDETMemberType.Constructor)
                .SelectMany(m => m.GetLinkedReturnTypes()).ToList();
            RemoveThisClassFromList(cadetClasses);
            return cadetClasses;
        }

        public List<CaDETClass> GetMethodLinkedVariableTypes()
        {
            var cadetClasses = Members.Where(m => m.Type is not CaDETMemberType.Property)
                .SelectMany(m => m.Variables)
                .SelectMany(v => v.GetLinkedTypes()).ToList();
            RemoveThisClassFromList(cadetClasses);
            return cadetClasses;
        }

        public List<CaDETClass> GetAccessedAccessorsTypes()
        {
            var accessedAccessors = Members.Where(m => m.Type is not CaDETMemberType.Property)
                .SelectMany(m => m.AccessedAccessors);
            var cadetClasses = accessedAccessors.Select(a => a.Parent).ToList();
            RemoveThisClassFromList(cadetClasses);
            return cadetClasses;
        }

        public List<CaDETClass> GetAccessedFieldsTypes()
        {
            var accessedFields = Members.Where(m => m.Type is not CaDETMemberType.Property)
                .SelectMany(m => m.AccessedFields);
            var cadetClasses = accessedFields.Select(f => f.Parent).ToList();
            RemoveThisClassFromList(cadetClasses);
            return cadetClasses;
        }

        public List<CaDETClass> GetMethodLinkedParameterTypes()
        {
            var parameters = Members.SelectMany(m => m.Params).ToList();
            var cadetClasses = parameters.Select(p => p.Type)
                .Where(v => v.LinkedTypes != null)
                .SelectMany(v => v.LinkedTypes).ToList();
            RemoveThisClassFromList(cadetClasses);
            return cadetClasses;
        }

        public List<CaDETClass> GetMethodInvocationsTypes()
        {
            var invokedMethods = Members.SelectMany(m => m.InvokedMethods).ToList();
            var cadetClasses = invokedMethods.Select(m => m.Parent).ToList();
            RemoveThisClassFromList(cadetClasses);
            return cadetClasses;
        }

        private void RemoveThisClassFromList(List<CaDETClass> classes)
        {
            classes.RemoveAll(c => c.Equals(this));
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

        public IEnumerable<CaDETMember> GetMethods()
        {
            return Members.Where(m => m.Type.Equals(CaDETMemberType.Method));
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
            return FullName.GetHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}