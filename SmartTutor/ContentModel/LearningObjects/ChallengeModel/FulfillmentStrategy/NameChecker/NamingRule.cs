using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

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
            foreach (string word in GetWordsFromNames(allNames))
            {
                if (word.Equals(requiredWord, StringComparison.OrdinalIgnoreCase))
                {
                    ++flag;
                    break;
                }
            }
            return flag != 0;
        }

        private bool CheckIfNotBannedWords(List<string> allNames)
        {
            foreach (string word in GetWordsFromNames(allNames))
                if (!CheckIfNotBannedWord(word)) return false;
            return true;
        }

        private bool CheckIfNotBannedWord(string word)
        {
            foreach (string bannedWord in BannedWords)
                if (word.Equals(bannedWord, StringComparison.OrdinalIgnoreCase)) return false;
            return true;
        }

        private List<string> GetWordsFromNames(List<string> names)
        {
            List<string> words = new List<string>();
            foreach (string name in names)
                words.AddRange(GetWordsFromName(name));
            return words;
        }

        private string[] GetWordsFromName(string name)
        {
            string[] words = GetWordByWordFromName(name);
            if (words.Length == 1) return words;
            return GetSubstringWordsFromName(name);
        }

        private string[] GetWordByWordFromName(string name)
        {
            var words = Regex.Split(name, "[A-Z]");
            var matches = Regex.Matches(name, "[A-Z]");
            for (int i = 0; i < words.Length - 1; i++)
                words[i + 1] = matches[i] + words[i + 1];
            return words.Where(val => val != "").ToArray();
        }

        private string[] GetSubstringWordsFromName(string name)
        {
            string[] words = GetWordByWordFromName(name);
            List<string> substringWords = new List<string>();
            substringWords.AddRange(words);
            int startLength = 0;
            for (var i = 0; i <= words.Length - 2; i++)
            {
                int endLength = words[i].Length;
                for (var j = i + 1; j <= words.Length - 1; j++)
                {
                    endLength += words[j].Length;
                    substringWords.Add(name.Substring(startLength, endLength));
                }
                startLength += words[i].Length;
            }
            return substringWords.ToArray();
        }
    }
}