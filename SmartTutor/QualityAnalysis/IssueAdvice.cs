using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.QualityAnalysis
{
    public class IssueAdvice
    {
        public int Id { get; private set; }
        public string IssueType { get; private set; }
        public List<LearningObjectSummary> Summaries { get; private set; }
    }
}