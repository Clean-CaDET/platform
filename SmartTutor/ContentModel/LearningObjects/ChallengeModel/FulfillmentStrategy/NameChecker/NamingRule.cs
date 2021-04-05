using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            foreach (string name in allNames)
                if (!CheckNameLength(name)) return false;
            return WordsMeetRequirements(allNames);
        }

        private bool CheckNameLength(string name)
        {
            return name.Length >= 2 && name.Length <= 25;
        }

        private bool WordsMeetRequirements(List<string> allNames)
        {
            return FoundAllRequiredWords(allNames) && CheckIfNotBannedAllWords(allNames);
        }

        private bool FoundAllRequiredWords(List<string> allNames)
        {
            foreach (string requiredWord in RequiredWords)
                if (!FoundRequiredWord(allNames, requiredWord)) return false;
            return true;
        }

        private bool FoundRequiredWord(List<string> allNames, string requiredWord)
        {
            return FoundWord(GetWordsFromNames(allNames), requiredWord) || FoundWord(allNames, requiredWord);
        }

        private bool FoundWord(List<string> words, string requiredWord)
        {
            int flag = 0;
            foreach (string word in words)
            {
                if (word.ToLower().Equals(requiredWord.ToLower()))
                {
                    ++flag;
                    break;
                }
            }
            return flag != 0;
        }

        private bool CheckIfNotBannedAllWords(List<string> allNames)
        {
            return CheckIfNotBannedWords(GetWordsFromNames(allNames)) && CheckIfNotBannedWords(allNames);
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
                if (word.ToLower().Equals(bannedWord.ToLower())) return false;
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
            var words = Regex.Split(name, "[A-Z]");
            var matches = Regex.Matches(name, "[A-Z]");
            for (int i = 0; i < words.Length - 1; i++)
                words[i + 1] = matches[i] + words[i + 1];
            return words;
        }
    }
}
