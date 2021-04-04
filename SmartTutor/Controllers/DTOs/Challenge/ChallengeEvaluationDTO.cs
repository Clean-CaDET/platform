using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Challenge
{
    public class ChallengeEvaluationDTO
    {
        public bool ChallengeCompleted { get; internal set; }
        public List<ChallengeHintDTO> ApplicableHints { get; internal set; }
    }
}
