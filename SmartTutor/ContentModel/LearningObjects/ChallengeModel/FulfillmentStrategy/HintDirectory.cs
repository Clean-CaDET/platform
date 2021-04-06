using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class HintDirectory
    {
        private readonly Dictionary<string, List<ChallengeHint>> _directory;

        public HintDirectory()
        {
            _directory = new Dictionary<string, List<ChallengeHint>>();
        }

        public Dictionary<string, List<ChallengeHint>> GetHints()
        {
            return new Dictionary<string, List<ChallengeHint>>(_directory);
        }

        public void AddHint(string codeSnippetId, ChallengeHint hint)
        {
            if (_directory.ContainsKey(codeSnippetId))
            {
                if (!_directory[codeSnippetId].Contains(hint))
                {
                    _directory[codeSnippetId].Add(hint);
                }
            }
            else
            {
                _directory[codeSnippetId] = new List<ChallengeHint> { hint };
            }
        }

        public void AddHints(string codeSnippetId, List<ChallengeHint> hints)
        {
            hints.ForEach(h => AddHint(codeSnippetId, h));
        }

        public void MergeHints(HintDirectory other)
        {
            foreach (var codeSnippetId in other._directory.Keys)
            {
                AddHints(codeSnippetId, other._directory[codeSnippetId]);
            }
        }

        public bool IsEmpty()
        {
            return _directory.Count == 0;
        }
    }
}
