using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETMemberMetrics
    {
        public int CYCLO { get; set; }
        public int LOC { get; set; }
        public List<CaDETMember> InvokedMethods { get; set; }
        //TODO: FDP, LAA, ATFD (http://www.simpleorientedarchitecture.com/how-to-identify-feature-envy-using-ndepend/) can be calculated
        //Contains fields and properties accessed by method belonging to this and other objects
        public List<CaDETMember> AccessedFieldsAndAccessors { get; set; }
    }
}