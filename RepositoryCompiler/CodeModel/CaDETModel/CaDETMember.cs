using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETMember
    {
        public string Name { get; set; }
        public CaDETMemberType Type { get; set; }
        public string SourceCode { get; set; }
        public CaDETClass Parent { get; set; }
        public CaDETMemberMetrics Metrics { get; set; }
        public ISet<CaDETMember> InvokedMethods { get; set; }
        //TODO: FDP, LAA, ATFD (http://www.simpleorientedarchitecture.com/how-to-identify-feature-envy-using-ndepend/) can be calculated
        //Contains fields and properties accessed by method belonging to this and other objects
        public ISet<CaDETMember> AccessedFieldsAndAccessors { get; set; }
        
        public override bool Equals(object? other)
        {
            if(!(other is CaDETMember otherMember)) return false;
            if(Parent == null) return Name.Equals(otherMember.Name);
            return Name.Equals(otherMember.Name) && Parent.Equals(otherMember.Parent);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}