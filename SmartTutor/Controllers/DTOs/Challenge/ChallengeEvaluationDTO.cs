using System.Collections.Generic;
using SmartTutor.Controllers.DTOs.Lecture;

namespace SmartTutor.Controllers.DTOs.Challenge
{
    public class ChallengeEvaluationDTO
    {
        public int ChallengeId { get; set; }
        public bool ChallengeCompleted { get; set; }
        public List<ChallengeHintDTO> ApplicableHints { get; set; }
        public LearningObjectDTO SolutionLO { get; set; }
    }
}
