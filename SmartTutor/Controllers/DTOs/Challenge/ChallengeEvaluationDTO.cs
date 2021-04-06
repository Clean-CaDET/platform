using System.Collections.Generic;
using SmartTutor.Controllers.DTOs.Lecture;

namespace SmartTutor.Controllers.DTOs.Challenge
{
    public class ChallengeEvaluationDTO
    {
        public bool ChallengeCompleted { get; set; }
        public Dictionary<string, List<ChallengeHintDTO>> ApplicableHints { get; set; }
        public List<LearningObjectDTO> ApplicableLOs { get; set; }
    }
}
