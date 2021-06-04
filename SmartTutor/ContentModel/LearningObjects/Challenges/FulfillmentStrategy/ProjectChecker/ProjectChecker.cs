using CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.ProjectChecker
{
    public class ProjectChecker : ChallengeFulfillmentStrategy
    {
        public List<SnippetStrategies> StrategiesApplicableToSnippet { get; private set; }

        private ProjectChecker() { }

        public ProjectChecker(List<SnippetStrategies> strategiesApplicableToSnippet) : this()
        {
            StrategiesApplicableToSnippet = strategiesApplicableToSnippet;
        }

        public override List<ChallengeHint> GetAllHints()
        {
            return StrategiesApplicableToSnippet.SelectMany(sas => sas.Strategies).SelectMany(cfs => cfs.GetAllHints()).ToList();
        }

        public override HintDirectory EvaluateSubmission(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var snippetStrategy in StrategiesApplicableToSnippet)
            {
                var codeSnippet = FindCodeSnippet(snippetStrategy.CodeSnippetId, solutionAttempt);
                foreach (var strategy in snippetStrategy.Strategies)
                {
                    challengeHints.MergeHints(strategy.EvaluateSubmission(new List<CaDETClass>() { codeSnippet }));
                }
            }
            return challengeHints;
        }

        private CaDETClass FindCodeSnippet(string codeSnippetId, List<CaDETClass> solutionAttempt)
        {
            foreach (var caDETClass in solutionAttempt.Where(caDETClass => codeSnippetId.Equals(caDETClass.FullName)))
                return caDETClass;

            foreach (var member in solutionAttempt.SelectMany(caDETClass => caDETClass.Members.Where(
                member => codeSnippetId.Equals(member.Signature()) && !member.Type.Equals(CaDETMemberType.Property))))
            {
                return new CaDETClass
                {
                    Members = new List<CaDETMember> { member },
                    FullName = "",
                    Name = "",
                    Fields = new List<CaDETField>()
                };
            }

            throw new KeyNotFoundException("Code snippet id wasn't found.");
        }
    }
}
