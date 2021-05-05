using CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.ProjectChecker
{
    public class ProjectAnalyzer : ChallengeFulfillmentStrategy
    {
        public Dictionary<string, List<ChallengeFulfillmentStrategy>> SnippetApplicableStrategies { get; private set; }

        private ProjectAnalyzer() { }

        public ProjectAnalyzer(Dictionary<string, List<ChallengeFulfillmentStrategy>> snippetApplicableStrategies) : this()
        {
            SnippetApplicableStrategies = snippetApplicableStrategies;
        }

        public override HintDirectory EvaluateSubmission(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var strategy in SnippetApplicableStrategies.SelectMany(sas => sas.Value))
            {
                var result = strategy.EvaluateSubmission(GetAllClassesFromSnippets(solutionAttempt));
                challengeHints.MergeHints(result);
            }
            return challengeHints;
        }

        public override List<ChallengeHint> GetAllHints()
        {
            return SnippetApplicableStrategies.SelectMany(sas => sas.Value).SelectMany(cfs => cfs.GetAllHints().Where(h => h != null)).ToList();
        }

        private List<CaDETClass> GetAllClassesFromSnippets(List<CaDETClass> solutionAttempt)
        {
            List<CaDETClass> classesFromSnippets = new List<CaDETClass>();
            classesFromSnippets.AddRange(GetClassesFromSnippets(solutionAttempt));
            classesFromSnippets.AddRange(GetMethodOnlyClassesFromSnippets(solutionAttempt));
            classesFromSnippets.AddRange(GetConstructorOnlyClassesFromSnippets(solutionAttempt));
            return classesFromSnippets;
        }

        private List<CaDETClass> GetClassesFromSnippets(List<CaDETClass> solutionAttempt)
        {
            return solutionAttempt.SelectMany(c => SnippetApplicableStrategies.Keys.Where(codeSnippetId => c.FullName.Equals(codeSnippetId)).Select(codeSnippetId => c)).ToList();
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
            List<CaDETClass> result = new List<CaDETClass>();
            foreach (var member in members.SelectMany(member => SnippetApplicableStrategies.Keys.Select(codeSnippetId => member)))
            {
                result.Add(new CaDETClass { Members = new List<CaDETMember> { member } });
            }
            return result;
        }
    }
}
