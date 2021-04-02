using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.NameChecker
{
    public class BasicNamesChecker : ChallengeFulfillmentStrategy
    {
        public List<string> BannedWords { get; internal set; }
        public List<string> RequiredWords { get; internal set; }

        public BasicNamesChecker(List<string> bannedWords, List<string> requiredWords)
        {
            BannedWords = bannedWords;
            RequiredWords = requiredWords;
            ChallengeHints = new List<ChallengeHint>();
        }

        public override ChallengeEvaluation CheckChallengeFulfillment(List<CaDETClass> solutionAttempt)
        {
            List<CaDETMember> submittedMethods = GetMethodsFromClasses(solutionAttempt);
            ChallengeEvaluation challengeEvaluation = new ChallengeEvaluation
            {
                ChallengeCompleted = ValidateNameRules(solutionAttempt),
                ApplicableHints = GetHintsForSolutionAttempt(solutionAttempt, submittedMethods)
            };
            return challengeEvaluation;
        }

        private bool ValidateNameRules(List<CaDETClass> caDETClasses)
        {
            // TODO: Check every word 
            List<CaDETMember> caDETMethods = GetMethodsFromClasses(caDETClasses);
            return ValidateClassNameRules(caDETClasses) && ValidateMethodNameRules(caDETMethods) && ValidateRequiredNameRules(caDETClasses, caDETMethods);
        }

        private List<CaDETMember> GetMethodsFromClasses(List<CaDETClass> caDETClasses)
        {
            return caDETClasses.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Method))).ToList();
        }

        private bool ValidateClassNameRules(List<CaDETClass> caDETClasses)
        {
            foreach (CaDETClass caDETClass in caDETClasses)
            {
                if (!NameMeetsRequirements(caDETClass.Name)) return false;
            }
            return true;
        }

        private bool ValidateMethodNameRules(List<CaDETMember> caDETMethods)
        {
            foreach (CaDETMember caDETMethod in caDETMethods)
            {
                if (!NameMeetsRequirements(caDETMethod.Name)) return false;
            }
            return true;
        }

        private bool NameMeetsRequirements(string name)
        {
            return CheckNameLength(name) && CheckIfBannedWord(name);
        }

        private bool CheckNameLength(string name)
        {
            return name.Length >= 2 && name.Length <= 20;
        }

        private bool CheckIfBannedWord(string name)
        {
            foreach (string bannedWord in BannedWords)
            {
                if (name.Equals(bannedWord)) return false;
            }
            return true;
        }

        private bool ValidateRequiredNameRules(List<CaDETClass> caDETClasses, List<CaDETMember> caDETMethods)
        {
            int flag = 0;
            foreach (string requiredWord in RequiredWords)
            {
                foreach (CaDETClass caDETClass in caDETClasses)
                {
                    if (requiredWord.Equals(caDETClass.Name)) ++flag;
                }

                foreach (CaDETMember caDETMethod in caDETMethods)
                {
                    if (requiredWord.Equals(caDETMethod.Name)) ++flag;
                }
            }
            return flag >= RequiredWords.Count();
        }

        // TODO: Add hint for solution attempt
        private List<ChallengeHint> GetHintsForSolutionAttempt(List<CaDETClass> solutionAttempt, List<CaDETMember> submittedMethods)
        {
            return new List<ChallengeHint>();
        }
    }
}
