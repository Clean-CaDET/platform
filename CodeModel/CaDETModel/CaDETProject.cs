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

        /// <summary>
        /// Finds the code snippet (class or member) and returns the related calculated code metrics.
        /// </summary>
        /// <param name="snippetId">The code snippet id is either the Full Name of a class or the Signature of a member.</param>
        /// <returns>A set of metrics calculated for the code snippet with the given ID. Null if no code snippet is found.</returns>
        public Dictionary<CaDETMetric, double> GetMetricsForCodeSnippet(string snippetId)
        {
            var classInstance = Classes.FirstOrDefault(c => c.FullName.Equals(snippetId));
            if (classInstance != null) return classInstance.Metrics;
            
            foreach (var c in Classes)
            {
                var memberInstance = c.Members.FirstOrDefault(m => m.ToString().Equals(snippetId));
                if (memberInstance != null) return memberInstance.Metrics;
            }

            return null;
        }
    }
}