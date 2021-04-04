using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Challenge
{
    public class ChallengeFulfillmentStrategyDTO
    {
        public int Id { get; set; }
        public List<ChallengeHintDTO> ChallengeHints { get; set; }
    }
}
