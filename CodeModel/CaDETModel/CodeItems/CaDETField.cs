using System.Collections.Generic;

namespace CodeModel.CaDETModel.CodeItems
{
    public class CaDETField
    {
        public string Name { get; internal set; }
        public List<CaDETModifier> Modifiers { get; internal set; }
        public CaDETClass Parent { get; internal set; }
        
        public CaDETLinkedType Type;

        public override bool Equals(object other)
        {
            if (!(other is CaDETField otherField)) return false;
            if (Parent == null) return Name.Equals(otherField.Name);
            return Name.Equals(otherField.Name) && Parent.Equals(otherField.Parent);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Parent.FullName + "." + Name;
        }

        public List<CaDETClass> GetLinkedTypes()
        {
            return Type.LinkedTypes;
        }
    }
}