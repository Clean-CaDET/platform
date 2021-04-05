using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.NameChecker
{
    [Table("BasicNamesChecker")]
    public class BasicNameChecker : ChallengeFulfillmentStrategy
    {
        public List<NamingRule> NamingRules { get; private set; }

        public BasicNameChecker() { }

        public BasicNameChecker(List<NamingRule> namingRules)
        {
            NamingRules = namingRules;
            ChallengeHints = GetChallengeHints();
        }

        public override ChallengeEvaluation CheckChallengeFulfillment(List<CaDETClass> solutionAttempt)
        {
            List<ChallengeHint> challengeHints = GetHintsForSolutionAttempt(solutionAttempt);
            if (!challengeHints.Any()) return new ChallengeEvaluation(true, ChallengeHints);
            return new ChallengeEvaluation(false, challengeHints);
        }

        private List<ChallengeHint> GetChallengeHints()
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            foreach (NamingRule namingRule in NamingRules)
                if (namingRule.Hint != null) challengeHints.Add(namingRule.Hint);
            return challengeHints;
        }

        private List<ChallengeHint> GetHintsForSolutionAttempt(List<CaDETClass> solutionAttempt)
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            List<string> allNames = GetAllNamesFromClasses(solutionAttempt);
            foreach (NamingRule namingRule in NamingRules)
            {
                if (!namingRule.NamesMeetRequirements(allNames) && namingRule.Hint != null)
                    challengeHints.Add(namingRule.Hint);
            }
            return challengeHints;
        }

        private List<string> GetAllNamesFromClasses(List<CaDETClass> caDETClasses)
        {
            List<string> allNames = new List<string>();
            foreach (CaDETClass caDETClass in caDETClasses)
                allNames.AddRange(GetClassPartsNames(caDETClass));
            return allNames;
        }

        private List<string> GetClassPartsNames(CaDETClass caDETClass)
        {
            List<string> classPartsNames = new List<string> { caDETClass.Name };
            classPartsNames.AddRange(GetClassFieldsNames(caDETClass.Fields));
            classPartsNames.AddRange(GetMembersPartsNames(caDETClass.Members));
            return classPartsNames;
        }

        private List<string> GetClassFieldsNames(List<CaDETField> caDETFields)
        {
            List<string> classFieldsNames = new List<string>();
            foreach (CaDETField caDETField in caDETFields)
                classFieldsNames.Add(caDETField.Name);
            return classFieldsNames;
        }

        private List<string> GetMembersPartsNames(List<CaDETMember> caDETMembers)
        {
            List<string> membersPartsNames = new List<string>();
            foreach (CaDETMember caDETMember in caDETMembers)
                membersPartsNames.AddRange(GetMemberPartsNames(caDETMember));
            return membersPartsNames;
        }

        private List<string> GetMemberPartsNames(CaDETMember caDETMember)
        {
            List<string> memberPartsNames = new List<string> { caDETMember.Name };
            memberPartsNames.AddRange(caDETMember.VariableNames);
            memberPartsNames.AddRange(GetMemberParametersNames(caDETMember.Params));
            return memberPartsNames;
        }

        private List<string> GetMemberParametersNames(List<CaDETParameter> caDETParameters)
        {
            List<string> memberParametersNames = new List<string>();
            foreach (CaDETParameter caDETParameter in caDETParameters)
                memberParametersNames.Add(caDETParameter.Name);
            return memberParametersNames;
        }
    }
}
