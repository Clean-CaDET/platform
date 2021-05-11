using CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.ProjectChecker
{
    public class ProjectChecker : ChallengeFulfillmentStrategy
    {
        public Dictionary<string, List<ChallengeFulfillmentStrategy>> StrategiesApplicableToSnippet { get; private set; }

        private ProjectChecker() { }

        public ProjectChecker(Dictionary<string, List<ChallengeFulfillmentStrategy>> strategiesApplicableToSnippet) : this()
        {
            StrategiesApplicableToSnippet = strategiesApplicableToSnippet;
        }

        public override List<ChallengeHint> GetAllHints()
        {
            return StrategiesApplicableToSnippet.SelectMany(sas => sas.Value).SelectMany(cfs => cfs.GetAllHints()).ToList();
        }

        public override HintDirectory EvaluateSubmission(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var codeSnippetId in StrategiesApplicableToSnippet.Keys)
            {
                var codeSnippet = FindCodeSnippet(codeSnippetId, solutionAttempt);
                foreach (var strategy in StrategiesApplicableToSnippet[codeSnippetId])
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
                string fullName = GetFullNameOfSnippet(codeSnippetId);
                return new CaDETClass
                {
                    Members = new List<CaDETMember> { member },
                    FullName = fullName,
                    Name = GetNameOfSnippet(fullName),
                    Fields = new List<CaDETField>()
                };
            }

            throw new KeyNotFoundException("Code snippet id wasn't found.");
        }

        private string GetFullNameOfSnippet(string codeSnippetId)
        {
            int memberNameLength = string.Concat(codeSnippetId.Reverse().ToArray().TakeWhile((c) => c != '.')).Length;
            return codeSnippetId.Substring(0, codeSnippetId.Length - memberNameLength - 1);
        }

        private string GetNameOfSnippet(string fullName)
        {
            string reversedNameOfSnippet = string.Concat(fullName.Reverse().ToArray().TakeWhile((c) => c != '.'));
            return new string(reversedNameOfSnippet.Reverse().ToArray());
        }
    }
}
