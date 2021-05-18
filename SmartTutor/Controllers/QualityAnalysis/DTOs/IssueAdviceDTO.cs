using System.Collections.Generic;

namespace SmartTutor.Controllers.QualityAnalysis.DTOs
{
    public class IssueAdviceDTO
    {
        public string IssueType { get; set; }
        public List<int> SummaryIds { get; set; }
    }
}