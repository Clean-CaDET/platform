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
            challengeHints.MergeHints(GetApplicableHintsForRequiredWords(solutionAttempt));
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
            {
                foreach (var namingRule in NamingRules)
                {
                    var result = namingRule.Evaluate(caDETClass.Name);
                    if (result == null) continue;
                    challengeHints.AddHint(caDETClass.FullName, result);
                }
            }
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForFieldNames(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var caDETField in GetFieldsFromClasses(solutionAttempt))
            {
                foreach (var namingRule in NamingRules)
                {
                    var result = namingRule.Evaluate(caDETField.Name);
                    if (result == null) continue;
                    challengeHints.AddHint(caDETField.Name, result);
                }
            }
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForMemberNames(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var caDETMember in GetMembersFromClasses(solutionAttempt))
            {
                foreach (var namingRule in NamingRules)
                {
                    var result = namingRule.Evaluate(caDETMember.Name);
                    if (result == null) continue;
                    challengeHints.AddHint(caDETMember.Signature(), result);
                }
            }
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForVariableNames(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var variableName in GetVariableNamesFromClasses(solutionAttempt))
            {
                foreach (var namingRule in NamingRules)
                {
                    var result = namingRule.Evaluate(variableName);
                    if (result == null) continue;
                    challengeHints.AddHint(variableName, result);
                }
            }
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForParameterNames(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var parameter in GetParametersFromClasses(solutionAttempt))
            {
                foreach (var namingRule in NamingRules)
                {
                    var result = namingRule.Evaluate(parameter.Name);
                    if (result == null) continue;
                    challengeHints.AddHint(parameter.Name, result);
                }
            }
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForRequiredWords(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var namingRule in NamingRules)
            {
                var requiredWords = namingRule.RequiredWords;
                var hint = namingRule.Hint;
                if (requiredWords.Count != 0 && hint != null)
                    challengeHints.AddHint(GetRequiredWordsCodeSnippetId(requiredWords), hint);
            }
            return challengeHints;
        }

        private string GetRequiredWordsCodeSnippetId(List<string> requiredWords)
        {
            string codeSnippetId = "";
            for (int i = 0; i < requiredWords.Count; i++)
            {
                codeSnippetId += requiredWords[i];
                if (i != requiredWords.Count - 1)
                    codeSnippetId += " ";
            }
            return codeSnippetId;
        }
    }
}
