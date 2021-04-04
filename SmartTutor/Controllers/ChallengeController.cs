using Microsoft.AspNetCore.Mvc;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
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

        [HttpPost("check")]
        public ChallengeCheckResponseDTO CheckChallengeCompletion([FromBody] ChallengeCheckRequestDTO checkRequestDto)
        {
            ChallengeEvaluation challengeEvaluation = _challengeService.CheckChallengeCompletion(checkRequestDto.SourceCode, checkRequestDto.ChallengeId);
            var text = challengeEvaluation.ChallengeCompleted ? "Success." : "Fail.";
            return new ChallengeCheckResponseDTO(text);
        }

        public List<ChallengeHint> GetAllHints(int challengeId)
        {
            return _challengeService.GetAllHints(challengeId);
        }

        public List<ChallengeHint> GetHintsForSolutionAttempt(int challengeId, List<CaDETClass> solutionAttempt)
        {
            return _challengeService.GetHintsForSolutionAttempt(challengeId, solutionAttempt);
        }
    }
}
