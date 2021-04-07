using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.Controllers.DTOs.Challenge;
using SmartTutor.Controllers.DTOs.Lecture;
using System.Collections.Generic;
using System.Linq;

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
            return Ok(_mapper.Map<ChallengeEvaluation, ChallengeEvaluationDTO>(challengeEvaluation, opt =>
            {
                opt.AfterMap((src, dest) =>
                {
                    var hintDirectory = src.ApplicableHints.GetDirectory();
                    var directoryKeys = src.ApplicableHints.GetHints();
                    foreach (var hintDto in dest.ApplicableHints)
                    {
                        var relatedHint = directoryKeys.First(h => h.Id == hintDto.Id);
                        hintDto.ApplicableToCodeSnippets = hintDirectory[relatedHint];
                        if (relatedHint.LearningObjectSummaryId == 0) continue;
                        var relatedLO = src.ApplicableLOs
                            .First(lo => lo.LearningObjectSummaryId == relatedHint.LearningObjectSummaryId);
                        hintDto.LearningObject = _mapper.Map<LearningObjectDTO>(relatedLO);
                    }
                });
            }));
        }

        public List<ChallengeHint> GetAllHints(int challengeId)
        {
            return _challengeService.GetAllHints(challengeId);
        }
    }
}
