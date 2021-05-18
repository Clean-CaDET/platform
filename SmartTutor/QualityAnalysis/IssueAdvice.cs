using SmartTutor.ContentModel.Lectures;
using System.Collections.Generic;

namespace SmartTutor.QualityAnalysis
{
    public class IssueAdvice
    {
        public int Id { get; private set; }
        public string IssueType { get; private set; }
        public List<LearningObjectSummary> Summaries { get; private set; }
    }
}