using CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.CaDETModel
{
    public class CaDETProject
    {
        private readonly LanguageEnum _language;

        public List<CaDETClass> Classes { get; }
        public List<string> SyntaxErrors { get; }

        public Dictionary<string, CodeLocationLink> CodeLinks { get; set; }
        
        public CaDETProject(LanguageEnum language, List<CaDETClass> classes, List<string> syntaxErrors)
        {
            _language = language;
            Classes = classes;
            SyntaxErrors = syntaxErrors;
        }

        public Dictionary<CaDETMetric, double> GetMetricsForCodeSnippet(string snippetId)
        {
            CaDETClass classInstance = Classes.FirstOrDefault(c => c.FullName.Equals(snippetId));
            if (classInstance != null) return classInstance.Metrics;
            
            CaDETMember memberInstance = null;
            foreach (var cl in Classes)
            {
                memberInstance = cl.Members.FirstOrDefault(m => m.ToString().Equals(snippetId));
                if (memberInstance != null) break;
            }
            return memberInstance.Metrics;
        }
    }
}