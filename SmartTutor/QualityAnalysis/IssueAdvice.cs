using SmartTutor.ContentModel.Lectures;
using System.Collections.Generic;

namespace SmartTutor.QualityAnalysis
{
    public class IssueAdvice
    {
        public string IssueType { get; set; }
        public List<LearningObjectSummary> Summaries { get; set; }
    }
}