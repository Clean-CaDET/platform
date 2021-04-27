using SmartTutor.Controllers.DTOs.Content;
using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.SubmissionEvaluation
{
    public class ChallengeEvaluationDTO
    {
        public int ChallengeId { get; set; }
        public bool ChallengeCompleted { get; set; }
        public List<ChallengeHintDTO> ApplicableHints { get; set; }
        public LearningObjectDTO SolutionLO { get; set; }
    }
}
