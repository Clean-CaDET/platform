using SmartTutor.Controllers.Content.DTOs;
using System;
using System.Collections.Generic;

namespace SmartTutor.Controllers.QualityAnalysis.DTOs
{
    public class CodeEvaluationDTO
    {
        public Dictionary<string, List<IssueAdviceDTO>> CodeSnippetIssueAdvice { get; set; }
        public ISet<LearningObjectDTO> LearningObjects { get; set; }
    }
}
