using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.Controllers.DTOs.Challenge;
using System.Collections.Generic;

namespace SmartTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly IChallengeService _challengeService;

        public ChallengeController()
        {
            _challengeService = new ChallengeService(new LearningObjectInMemoryRepository());
        }

        [HttpPost("evaluate-submission")]
        public ChallengeEvaluationDTO EvaluateChallengeSubmission([FromBody] ChallengeSubmissionDTO challengeSubmission)
        {
            ChallengeEvaluation challengeEvaluation = _challengeService.EvaluateSubmission(challengeSubmission.SourceCode, challengeSubmission.ChallengeId);
            return new ChallengeEvaluationDTO();
        }

        public List<ChallengeHint> GetAllHints(int challengeId)
        {
            return _challengeService.GetAllHints(challengeId);
        }
    }
}
