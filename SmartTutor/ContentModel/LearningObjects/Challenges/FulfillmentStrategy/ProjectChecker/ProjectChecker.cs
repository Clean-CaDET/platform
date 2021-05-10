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

        public override HintDirectory EvaluateSubmission(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var strategy in StrategiesApplicableToSnippet.SelectMany(sas => sas.Value))
            {
                var result = strategy.EvaluateSubmission(GetAllClassesFromSnippets(solutionAttempt));
                challengeHints.MergeHints(result);
            }
            return challengeHints;
        }

        public override List<ChallengeHint> GetAllHints()
        {
            return StrategiesApplicableToSnippet.SelectMany(sas => sas.Value).SelectMany(cfs => cfs.GetAllHints()).ToList();
        }

        private List<CaDETClass> GetAllClassesFromSnippets(List<CaDETClass> solutionAttempt)
        {
            List<CaDETClass> classesFromSnippets = new();
            classesFromSnippets.AddRange(GetRegularClassesFromSnippets(solutionAttempt));
            classesFromSnippets.AddRange(GetMethodOnlyClassesFromSnippets(solutionAttempt));
            classesFromSnippets.AddRange(GetConstructorOnlyClassesFromSnippets(solutionAttempt));
            return classesFromSnippets;
        }

        private List<CaDETClass> GetRegularClassesFromSnippets(List<CaDETClass> solutionAttempt)
        {
            return solutionAttempt.SelectMany(c => StrategiesApplicableToSnippet.Keys.Where(codeSnippetId => c.FullName.Equals(codeSnippetId)).Select(codeSnippetId => c)).ToList();
        }

        private List<CaDETClass> GetMethodOnlyClassesFromSnippets(List<CaDETClass> solutionAttempt)
        {
            var methods = solutionAttempt.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Method))).ToList();
            return GetMemberOnlyClassesFromSnippets(methods);
        }

        private List<CaDETClass> GetConstructorOnlyClassesFromSnippets(List<CaDETClass> solutionAttempt)
        {
            var constructors = solutionAttempt.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Constructor))).ToList();
            return GetMemberOnlyClassesFromSnippets(constructors);
        }

        private List<CaDETClass> GetMemberOnlyClassesFromSnippets(List<CaDETMember> members)
        {
            List<CaDETClass> result = new();
            foreach (var member in members.SelectMany(member => StrategiesApplicableToSnippet.Keys.Select(codeSnippetId => member)))
            {
                result.Add(new CaDETClass { Members = new List<CaDETMember> { member } });
            }
            return result;
        }
    }
}
