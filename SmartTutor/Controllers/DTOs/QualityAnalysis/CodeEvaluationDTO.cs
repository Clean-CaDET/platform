using SmartTutor.Controllers.DTOs.Content;
using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.QualityAnalysis
{
    public class CodeEvaluationDTO
    {
        public Dictionary<string, List<IssueAdviceDTO>> CodeSnippetIssueAdvice { get; set; }
        public ISet<LearningObjectDTO> LearningObjects { get; set; }
    }
}
