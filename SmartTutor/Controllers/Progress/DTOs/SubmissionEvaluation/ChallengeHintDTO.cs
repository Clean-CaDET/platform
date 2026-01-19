using SmartTutor.Controllers.Content.DTOs;
using System.Collections.Generic;

namespace SmartTutor.Controllers.Progress.DTOs.SubmissionEvaluation
{
    public class ChallengeHintDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public LearningObjectDTO LearningObject { get; set; }
        public List<string> ApplicableToCodeSnippets { get; set; }
    }
}
