using RepositoryCompiler.CodeModel.CaDETModel.Metrics;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCompiler.CodeModel.CaDETModel.CodeItems
{
    public class CaDETMember
    {
        public string Name { get; internal set; }
        public CaDETMemberType Type { get; internal set; }
        public string SourceCode { get; internal set; }
        public CaDETClass Parent { get; internal set; }
        public List<CaDETParameter> Params { get; internal set; }
        public List<CaDETModifier> Modifiers { get; internal set; }
        public CaDETMemberMetrics Metrics { get; internal set; }
        public ISet<CaDETMember> InvokedMethods { get; internal set; }
        public ISet<CaDETMember> AccessedAccessors { get; internal set; }
        public ISet<CaDETField> AccessedFields { get; internal set; }

        public string GetSignature()
        {
            var signatureBuilder = new StringBuilder();
            if (Parent != null) signatureBuilder.Append(Parent.FullName).Append(".");
            signatureBuilder.Append(Name);
            if (Params != null)
            {
                signatureBuilder.Append("(");
                for (var i = 0; i < Params.Count; i++)
                {
                    signatureBuilder.Append(Params[i].Type);
                    if (i < Params.Count - 1) signatureBuilder.Append(", ");
                }
                signatureBuilder.Append(")");
            }

            return signatureBuilder.ToString();
        }

        public bool IsFieldDefiningAccessor()
        {
            //TODO: This is a workaround that should be reworked https://stackoverflow.com/questions/64009302/roslyn-c-how-to-get-all-fields-and-properties-and-their-belonging-class-acce
            //TODO: It is specific to C# properties. Should move this to CSharpCodeParser so that each language can define its rule for calculating simple accessors.
            //In its current form, this function will return true for simple properties (e.g., public int SomeNumber { get; set; })
            return Type.Equals(CaDETMemberType.Property)
                   && (InvokedMethods.Count == 0)
                   && (AccessedAccessors.Count == 0 && AccessedFields.Count == 0)
                   && !SourceCode.Contains("return ") && !SourceCode.Contains("=");
        }

        public List<CaDETField> GetAccessedOwnFields()
        {
            List<CaDETField> accessedOwnFields = new List<CaDETField>();

            foreach (var accessedOwnField in AccessedFields)
            {
                if (accessedOwnField.Parent == Parent)
                {
                    accessedOwnFields.Add(accessedOwnField);
                }
            }

            return accessedOwnFields;
        }

        public List<CaDETMember> GetAccessedOwnAccessors() {
            List<CaDETMember> accessedOwnAccessors = new List<CaDETMember>();

            foreach (var accessedOwnAccessor in AccessedAccessors)
            {
                if (accessedOwnAccessor.Parent == Parent)
                {
                    accessedOwnAccessors.Add(accessedOwnAccessor);
                }
            }
            return accessedOwnAccessors;
        }

        public override bool Equals(object other)
        {
            if (!(other is CaDETMember otherMember)) return false;
            return Parent.Equals(otherMember.Parent) && GetSignature().Equals(otherMember.GetSignature());
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return GetSignature();
        }
    }
}