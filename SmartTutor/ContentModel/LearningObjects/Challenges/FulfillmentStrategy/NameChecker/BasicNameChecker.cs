using CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.NameChecker
{
    public class BasicNameChecker : ChallengeFulfillmentStrategy
    {
        //TODO: Delegate to several rule types as our understanding grows - e.g., NameLengthRule, RequiredWordRule. For now we are going with the simplest solution.
        //TODO: Currently this checker should either define (some subset of) banned OR required words and make a related hint.
        public List<string> BannedWords { get; private set; }
        public List<string> RequiredWords { get; private set; }
        public ChallengeHint Hint { get; private set; }

        private BasicNameChecker() {}
        public BasicNameChecker(List<string> bannedWords, List<string> requiredWords, ChallengeHint hint): this()
        {
            BannedWords = bannedWords;
            RequiredWords = requiredWords;
            Hint = hint;
        }

        public override HintDirectory EvaluateSubmission(List<CaDETClass> solutionAttempt)
        {
            var usedNames = GetUsedNames(solutionAttempt);
            return EvaluateNames(usedNames);
        }

        private Dictionary<string, List<string>> GetUsedNames(List<CaDETClass> solutionAttempt)
        {
            var namesUsedInCodeSnippet = new Dictionary<string, List<string>>();

            solutionAttempt.ForEach(c => namesUsedInCodeSnippet.Add(c.FullName, GetClassNames(c)));

            foreach (var member in GetMembersFromClasses(solutionAttempt))
            {
                namesUsedInCodeSnippet.Add(member.Signature(), GetMemberNames(member));
            }

            return namesUsedInCodeSnippet;
        }

        private List<string> GetClassNames(CaDETClass caDETClass)
        {
            var names = new List<string> { caDETClass.Name };
            names.AddRange(caDETClass.Fields.Select(f => f.Name));
            return names;
        }

        private List<CaDETMember> GetMembersFromClasses(List<CaDETClass> classes)
        {
            return classes.SelectMany(c => c.Members).ToList();
        }

        private List<string> GetMemberNames(CaDETMember member)
        {
            var memberNames = new List<string> { member.Name };
            memberNames.AddRange(member.Variables.Select(v => v.Name));
            memberNames.AddRange(member.Params.Select(p => p.Name));
            return memberNames;
        }

        private HintDirectory EvaluateNames(Dictionary<string, List<string>> namesUsedInCodeSnippet)
        {
            var hints = new HintDirectory();
            hints.MergeHints(EvaluateBannedWords(namesUsedInCodeSnippet));
            hints.MergeHints(EvaluateRequiredWords(namesUsedInCodeSnippet));
            return hints;
        }

        private HintDirectory EvaluateBannedWords(Dictionary<string, List<string>> namesUsedInCodeSnippet)
        {
            if (BannedWords == null || BannedWords.Count == 0) return null;

            var hints = new HintDirectory();
            foreach (var codeSnippetId in namesUsedInCodeSnippet.Keys)
            {
                if (ContainsBannedName(namesUsedInCodeSnippet[codeSnippetId]))
                    hints.AddHint(codeSnippetId, Hint);
            }

            return hints;
        }

        private bool ContainsBannedName(List<string> names)
        {
            foreach (var word in names.SelectMany(GetWordsFromName))
            {
                if (BannedWords.Contains(word, StringComparer.OrdinalIgnoreCase)) return true;
            }

            return false;
        }

        private HintDirectory EvaluateRequiredWords(Dictionary<string, List<string>> namesUsedInCodeSnippet)
        {
            if (RequiredWords == null || RequiredWords.Count == 0) return null;
            
            var allNames = namesUsedInCodeSnippet.Values.SelectMany(n => n);
            var allWords = allNames.SelectMany(GetWordsFromName).ToList();
            if (RequiredWords.All(req => allWords.Contains(req, StringComparer.OrdinalIgnoreCase))) return null;

            var hints = new HintDirectory();
            hints.AddHint("ALL", Hint);
            return hints;
        }

        private string[] GetWordsFromName(string name)
        {
            var words = Regex.Split(name, "[A-Z]");

            var matches = Regex.Matches(name, "[A-Z]");
            for (int i = 0; i < words.Length - 1; i++)
            {
                words[i + 1] = matches[i] + words[i + 1];
            }

            var singleWords = words.Where(val => val != "").ToArray();
            var allWords = GetSyntagmFromName(name, singleWords);
            allWords.AddRange(singleWords);
            return allWords.Distinct().ToArray();
        }

        private List<string> GetSyntagmFromName(string name, string[] words)
        {
            List<string> syntagms = new List<string>();
            int startLength = 0;
            for (var i = 0; i <= words.Length - 2; i++)
            {
                int endLength = words[i].Length;
                for (var j = i + 1; j <= words.Length - 1; j++)
                {
                    endLength += words[j].Length;
                    syntagms.Add(name.Substring(startLength, endLength));
                }
                startLength += words[i].Length;
            }
            return syntagms;
        }

        public override List<ChallengeHint> GetAllHints()
        {
            return new List<ChallengeHint> { Hint };
        }
    }
}
