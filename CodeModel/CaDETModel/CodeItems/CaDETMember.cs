﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeModel.CaDETModel.CodeItems
{
    public class CaDETMember
    {
        public string Name { get; internal set; }
        public CaDETMemberType Type { get; internal set; }
        public string SourceCode { get; internal set; }
        public CaDETClass Parent { get; internal set; }
        public List<CaDETParameter> Params { get; internal set; }
        public List<CaDETModifier> Modifiers { get; internal set; }
        public Dictionary<CaDETMetric, double> Metrics { get; internal set; }
        public ISet<CaDETMember> InvokedMethods { get; internal set; }
        public ISet<CaDETMember> AccessedAccessors { get; internal set; }
        public ISet<CaDETField> AccessedFields { get; internal set; }
        public CaDETLinkedType ReturnType;
        public List<CaDETVariable> Variables { get; internal set; } = new List<CaDETVariable>();

        public string Signature()
        {
            var signatureBuilder = new StringBuilder();
            if (Parent != null) signatureBuilder.Append(Parent.FullName).Append(".");
            signatureBuilder.Append(Name);
            if (Params != null)
            {
                signatureBuilder.Append("(");
                for (var i = 0; i < Params.Count; i++)
                {
                    signatureBuilder.Append(Params[i].Type.FullType);
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

        public List<CaDETField> GetDirectlyAndIndirectlyAccessedOwnFields()
        {
            HashSet<CaDETMember> members = new HashSet<CaDETMember>(){this};

            return FindAccessedOwnFields(members).ToList();
        }

        private HashSet<CaDETField> FindAccessedOwnFields(HashSet<CaDETMember> members)
        {
            HashSet<CaDETField> fields =  GetAccessedOwnFields().ToHashSet();
            foreach (var method in InvokedMethods.Where(m => m.Parent.Equals(Parent)))
            {
                if (members.Contains(method)) continue;

                HashSet<CaDETField> foundFields = method.FindAccessedOwnFields(members);
                fields.UnionWith(foundFields);
                members.Add(method);
            }

            return fields;
        }

        /// <summary>
        /// Member is normal method if it's neither constructor, nor getter or setter,
        /// nor delegate function. TODO accesses multiple fields
        /// </summary>
        public bool IsMemberNormalMethod()
        {
            return Type == CaDETMemberType.Method && Metrics[CaDETMetric.MELOC] > 1 && IsMemberPublic();
        }

        private bool IsMemberPublic()
        {
            return Modifiers.Exists(md => md.Value == CaDETModifierValue.Public);
        }

        public override bool Equals(object other)
        {
            if (!(other is CaDETMember otherMember)) return false;
            return Parent.Equals(otherMember.Parent) && Signature().Equals(otherMember.Signature());
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Signature();
        }

        public List<CaDETClass> GetLinkedReturnTypes()
        {
            if (ReturnType == null) return new List<CaDETClass>();
            return ReturnType.LinkedTypes;
        }
    }
}