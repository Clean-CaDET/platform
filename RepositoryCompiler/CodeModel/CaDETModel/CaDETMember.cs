using System;
using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETMember
    {
        public string Name { get; set; }
        public CaDETMemberType Type { get; set; }
        public string SourceCode { get; set; }
        public CaDETClass Parent { get; set; }

        public List<CaDETMember> InvokedMethods { get; set; }
        //TODO: FDP, LAA, ATFD (http://www.simpleorientedarchitecture.com/how-to-identify-feature-envy-using-ndepend/) can be calculated
        //Contains fields and properties accessed by method belonging to this and other objects
        public List<CaDETMember> AccessedFieldsAndAccessors { get; set; }
        public CaDETMemberMetrics Metrics { get; set; }
    }
}