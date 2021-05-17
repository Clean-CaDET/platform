using SmartTutor.Controllers.DTOs.Content;
using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.QualityAnalysis
{
    public class IssueAdviceDTO
    {
        public string IssueType { get; set; }
        public List<LearningObjectSummaryDTO> Summaries { get; set; }
    }
}