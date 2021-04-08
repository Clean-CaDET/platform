using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.NameChecker
{
    [Table("BasicNameCheckers")]
    public class BasicNameChecker : ChallengeFulfillmentStrategy
    {
        public List<NamingRule> NamingRules { get; set; }

        public override HintDirectory EvaluateSubmission(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = GetApplicableHintsForClassNames(solutionAttempt);
            challengeHints.MergeHints(GetApplicableHintsForFieldNames(solutionAttempt));
            challengeHints.MergeHints(GetApplicableHintsForMemberNames(solutionAttempt));
            challengeHints.MergeHints(GetApplicableHintsForVariableNames(solutionAttempt));
            challengeHints.MergeHints(GetApplicableHintsForParameterNames(solutionAttempt));
            challengeHints.MergeHints(GetApplicableHintsForRequiredWords());
            return challengeHints;
        }

        public override List<ChallengeHint> GetAllHints()
        {
            var challengeHints = new List<ChallengeHint>();
            challengeHints.AddRange(NamingRules.Select(nr => nr.Hint));
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForClassNames(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var caDETClass in solutionAttempt)
                challengeHints.MergeHints(EvaluateName(caDETClass.Name, caDETClass.FullName));
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForFieldNames(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var field in GetFieldsFromClasses(solutionAttempt))
                challengeHints.MergeHints(EvaluateName(field.Name, field.Parent.FullName));
            return challengeHints;
        }

        private List<CaDETField> GetFieldsFromClasses(List<CaDETClass> classes)
        {
            return classes.SelectMany(c => c.Fields).ToList();
        }

        private HintDirectory GetApplicableHintsForMemberNames(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var caDETMember in GetMembersFromClasses(solutionAttempt))
                challengeHints.MergeHints(EvaluateName(caDETMember.Name, caDETMember.Signature()));
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForVariableNames(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var (member, variableName) in GetMembersFromClasses(solutionAttempt).SelectMany(m => m.VariableNames.Select(vn => (m, vn))))
                challengeHints.MergeHints(EvaluateName(variableName, member.Signature()));
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForParameterNames(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var (member, parameter) in GetMembersFromClasses(solutionAttempt).SelectMany(m => m.Params.Select(p => (m, p))))
                challengeHints.MergeHints(EvaluateName(parameter.Name, member.Signature()));
            return challengeHints;
        }

        private List<CaDETMember> GetMembersFromClasses(List<CaDETClass> classes)
        {
            return classes.SelectMany(c => c.Members).ToList();
        }

        private HintDirectory EvaluateName(string name, string codeSnippetId)
        {
            var challengeHints = new HintDirectory();
            foreach (var namingRule in NamingRules)
            {
                var result = namingRule.Evaluate(name);
                if (result == null) continue;
                challengeHints.AddHint(codeSnippetId, result);
            }
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForRequiredWords()
        {
            var challengeHints = new HintDirectory();
            foreach (var namingRule in NamingRules)
            {
                foreach (var requiredWord in namingRule.RequiredWords)
                {
                    if (namingRule.Hint == null) continue;
                    challengeHints.AddHint(requiredWord, namingRule.Hint);
                }
            }
            return challengeHints;
        }
    }
}
