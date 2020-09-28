using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETClass
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string SourceCode { get; set; }
        public List<CaDETMember> Methods { get; set; }
        public List<CaDETMember> Fields { get; set; }

        public int MetricLOC { get; set; }
        public double MetricLCOM { get; set; }

        public int MetricNMD()
        {
            return Methods.Count;
        }

        public int MetricNAD()
        {
            return Fields.Count;
        }

        public int MetricWMC()
        {
            return Methods.Sum(method => method.MetricCYCLO);
        }
    }
}