using System.Collections.Generic;

namespace CodeModel.CaDETModel.CodeItems
{
    public class CaDETVariable
    {
        public string Name { get; set; }
        public CaDETLinkedType Type;

        public CaDETVariable(string name, CaDETLinkedType type)
        {
            Name = name;
            Type = type;
        }
        
        public List<CaDETClass> GetLinkedTypes()
        {
            return Type.LinkedTypes;
        }
    }
}