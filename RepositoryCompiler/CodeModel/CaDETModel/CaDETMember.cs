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
        // TODO: Make model for Params
        public List<string> Params { get; set; }
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

        public bool IsSimpleAccessor()
        {
            return Type.Equals(CaDETMemberType.Property)
                   && (InvokedMethods == null || InvokedMethods.Count == 0)
                   && (AccessedFieldsAndAccessors == null || AccessedFieldsAndAccessors.Count == 0)
                   && !SourceCode.Contains("return ") && !SourceCode.Contains("="); //TODO: This is a workaround that should be reworked https://stackoverflow.com/questions/64009302/roslyn-c-how-to-get-all-fields-and-properties-and-their-belonging-class-acce
                                                                                    //TODO: It is specific to C# properties. Should move this to CSharpCodeParser so that each language can define its rule for calculating simple accessors.
        }
    }
}