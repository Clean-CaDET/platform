using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.Lectures;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.QualityAnalysis
{
    public class CodeQualityEvaluation
    {
        private readonly Dictionary<string, List<IssueAdvice>> _codeSnippetIssueAdvice;
        public ISet<LearningObject> LearningObjects { get; private set; }

        public CodeQualityEvaluation()
        {
            _codeSnippetIssueAdvice = new Dictionary<string, List<IssueAdvice>>();
            LearningObjects = new HashSet<LearningObject>();
        }

        public void Put(string codeSnippetId, List<IssueAdvice> issuesAndSummaries)
        {
            _codeSnippetIssueAdvice.Add(codeSnippetId, issuesAndSummaries);
        }

        public List<LearningObjectSummary> GetLearningObjectSummaries()
        {
            var allAdvice = _codeSnippetIssueAdvice.Values.SelectMany(advice => advice);
            return allAdvice.SelectMany(advice => advice.Summaries).Distinct().ToList();
        }

        //TODO: Current understanding is that this is a shallow copy that does not protect the Lists (i.e., their reference is maintained)
        public Dictionary<string, List<IssueAdvice>> GetIssueAdvice()
        {
            return new Dictionary<string, List<IssueAdvice>>(_codeSnippetIssueAdvice);
        }

        public void AddLearningObjects(List<LearningObject> instructorLOs)
        {
            LearningObjects.UnionWith(instructorLOs);
        }
    }
}