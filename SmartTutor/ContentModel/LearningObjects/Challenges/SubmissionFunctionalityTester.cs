using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel
{
    public class ChallengeSubmissionFunctionalityTester
    {
        public List<string> Test(string sourceCode)
        {
            var syntaxErrors = new List<string>();
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            
            var diagnostics = syntaxTree.GetDiagnostics().ToList();
            if (diagnostics.Count > 0)
            {
                foreach (var diagnostic in diagnostics)
                {
                    syntaxErrors.Add(diagnostic.ToString());
                }
            }

            return syntaxErrors;
        }
    }
}