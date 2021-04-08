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
        public int MaxLength { get; set; } = 35;
        public int MinLength { get; set; } = 2;

        internal ChallengeHint Evaluate(string name)
        {
            return NameMeetRequirements(name) ? null : Hint;
        }

        private bool NameMeetRequirements(string name)
        {
            return CheckNameLength(name) && CheckNameContent(name);
        }

        private bool CheckNameLength(string name)
        {
            return name.Length >= MinLength && name.Length <= MaxLength;
        }

        private bool CheckNameContent(string name)
        {
            CheckIfNameIsRequiredWord(name);
            return CheckIfNotBannedName(name);
        }

        private void CheckIfNameIsRequiredWord(string name)
        {
            foreach (string word in GetWordsFromName(name))
                CheckIfWordIsRequired(word);
        }

        private void CheckIfWordIsRequired(string name)
        {
            foreach (string requiredWord in RequiredWords)
            {
                if (name.Equals(requiredWord, StringComparison.OrdinalIgnoreCase))
                {
                    RemoveRequiredWord(requiredWord);
                    break;
                }
            }
        }

        private void RemoveRequiredWord(string requiredWord)
        {
            RequiredWords.Remove(requiredWord);
        }

        private bool CheckIfNotBannedName(string name)
        {
            foreach (string word in GetWordsFromName(name))
                if (!CheckIfNotBannedWord(word)) return false;
            return true;
        }

        private bool CheckIfNotBannedWord(string word)
        {
            foreach (string bannedWord in BannedWords)
                if (word.Equals(bannedWord, StringComparison.OrdinalIgnoreCase)) return false;
            return true;
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
            List<string> substringWords = GetMultipartSubstringWordsFromName(name, words);
            substringWords.AddRange(words);
            return substringWords.ToArray();
        }

        private List<string> GetMultipartSubstringWordsFromName(string name, string[] words)
        {
            List<string> substringWords = new List<string>();
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
            return substringWords;
        }
    }
}