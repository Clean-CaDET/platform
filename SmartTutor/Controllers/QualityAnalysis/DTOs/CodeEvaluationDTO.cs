using SmartTutor.Controllers.Content.DTOs;
using System;
using System.Collections.Generic;

namespace SmartTutor.Controllers.QualityAnalysis.DTOs
{
    public class CodeEvaluationDTO
    {
        public Dictionary<string, List<IssueAdviceDTO>> CodeSnippetIssueAdvice { get; set; }
        public ISet<LearningObjectDTO> LearningObjects { get; set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CodeSnippetIssueAdvice, LearningObjects);
        }
    }
}
