using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.NameChecker
{
    public class NamingRule
    {
        [Key] public int Id { get; set; }
        public List<string> BannedWords { get; set; }
        public List<string> RequiredWords { get; set; }
        public ChallengeHint Hint { get; set; }

        internal bool NamesMeetRequirements(List<string> allNames)
        {
            return CheckNamesLength(allNames) && CheckNamesContent(allNames);
        }

        private bool CheckNamesLength(List<string> allNames)
        {
            foreach (string name in allNames)
                if (!CheckNameLength(name)) return false;
            return true;
        }

        private bool CheckNameLength(string name)
        {
            return name.Length >= 2 && name.Length <= 25;
        }

        private bool CheckNamesContent(List<string> allNames)
        {
            return FoundAllRequiredWords(allNames) && CheckIfNotBannedWords(allNames);
        }

        private bool FoundAllRequiredWords(List<string> allNames)
        {
            foreach (string requiredWord in RequiredWords)
                if (!FoundRequiredWord(allNames, requiredWord)) return false;
            return true;
        }

        private bool FoundRequiredWord(List<string> allNames, string requiredWord)
        {
            int flag = 0;
            foreach (string name in allNames)
            {
                if (name.Contains(requiredWord, StringComparison.OrdinalIgnoreCase))
                {
                    ++flag;
                    break;
                }
            }
            return flag != 0;
        }

        private bool CheckIfNotBannedWords(List<string> words)
        {
            foreach (string word in words)
                if (!CheckIfNotBannedWord(word)) return false;
            return true;
        }

        private bool CheckIfNotBannedWord(string word)
        {
            foreach (string bannedWord in BannedWords)
                if (word.Contains(bannedWord, StringComparison.OrdinalIgnoreCase)) return false;
            return true;
        }
    }
}
