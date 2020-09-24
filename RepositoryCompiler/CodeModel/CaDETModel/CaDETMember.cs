using System;
using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETMember
    {
        private int? _linesOfCode;
        public string Name { get; set; }
        public CaDETMemberType Type { get; set; }
        public string SourceCode { get; set; }
        public CaDETClass Parent { get; set; }
        public int MetricCYCLO { get; set; }
        public List<CaDETMember> InvokedMethods { get; set; }
        //TODO: FDP, LAA, ATFD (http://www.simpleorientedarchitecture.com/how-to-identify-feature-envy-using-ndepend/) can be calculated
        //Contains fields and properties accessed by method belonging to this and other objects
        public List<CaDETMember> AccessedFields { get; set; }

        public int MetricLOC()
        {
            _linesOfCode ??= SourceCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
            return _linesOfCode.Value;
        }
    }
}