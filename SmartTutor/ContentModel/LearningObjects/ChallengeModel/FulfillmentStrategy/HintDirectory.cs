using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class HintDirectory
    {
        private readonly Dictionary<ChallengeHint, List<string>> _directory;

        public HintDirectory()
        {
            _directory = new Dictionary<ChallengeHint, List<string>>();
        }

        public Dictionary<ChallengeHint, List<string>> GetDirectory()
        {
            return new Dictionary<ChallengeHint, List<string>>(_directory);
        }

        public List<ChallengeHint> GetHints()
        {
            return _directory.Keys.ToList();
        }

        public void AddHint(string codeSnippetId, ChallengeHint hint)
        {
            if (_directory.ContainsKey(hint))
            {
                if (!_directory[hint].Contains(codeSnippetId))
                {
                    _directory[hint].Add(codeSnippetId);
                }
            }
            else
            {
                _directory[hint] = new List<string> { codeSnippetId };
            }
        }

        public void AddHints(ChallengeHint hint, List<string> codeSnippetIds)
        {
            codeSnippetIds.ForEach(codeSnippetId => AddHint(codeSnippetId, hint));
        }

        public void MergeHints(HintDirectory other)
        {
            if (other == null) return;
            foreach (var hint in other._directory.Keys)
            {
                AddHints(hint, other._directory[hint]);
            }
        }

        public List<int> GetDistinctLearningObjectSummaries()
        {
            return _directory.Keys.Select(hint => hint.LearningObjectSummaryId)
                .Where(id => id != 0).Distinct().ToList();
        }

        public bool IsEmpty()
        {
            return _directory.Count == 0;
        }
    }
}
