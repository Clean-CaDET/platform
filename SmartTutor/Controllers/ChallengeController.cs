using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.Controllers.DTOs.Challenge;
using System.Collections.Generic;

namespace SmartTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChallengeService _challengeService;

        public ChallengeController(IMapper mapper, IChallengeService service)
        {
            _mapper = mapper;
            _challengeService = service;
        }

        [HttpPost("evaluate-submission")]
        public ActionResult<ChallengeEvaluationDTO> EvaluateChallengeSubmission([FromBody] ChallengeSubmissionDTO challengeSubmission)
        {
            var challengeEvaluation = _challengeService.EvaluateSubmission(challengeSubmission.SourceCode, challengeSubmission.ChallengeId);
            if (challengeEvaluation == null) return NotFound();
            return Ok(_mapper.Map<ChallengeEvaluationDTO>(challengeEvaluation));
        }

        public List<ChallengeHint> GetAllHints(int challengeId)
        {
            return _challengeService.GetAllHints(challengeId);
        }
    }
}
