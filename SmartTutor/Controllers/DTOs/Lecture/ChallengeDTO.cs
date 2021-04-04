using SmartTutor.Controllers.DTOs.Challenge;

namespace SmartTutor.Controllers.DTOs.Lecture
{
    public class ChallengeDTO : LearningObjectDTO
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public ChallengeFulfillmentStrategyDTO FulfillmentStrategy { get; set; }
    }
}