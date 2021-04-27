using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.Controllers.DTOs.Content;
using SmartTutor.Controllers.DTOs.SubmissionEvaluation;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Submissions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.Controllers
{
    [Route("api/submissions/")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISubmissionService _submissionService;

        public SubmissionController(IMapper mapper, ISubmissionService service)
        {
            _mapper = mapper;
            _submissionService = service;
        }

        [HttpPost("challenge")]
        public ActionResult<ChallengeEvaluationDTO> EvaluateChallengeSubmission([FromBody] ChallengeSubmissionDTO challengeSubmission)
        {
            ChallengeEvaluation challengeEvaluation;
            try
            {
                challengeEvaluation = _submissionService.EvaluateChallenge(_mapper.Map<ChallengeSubmission>(challengeSubmission));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            
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
                        if (relatedHint.LearningObjectSummaryId == null) continue;
                        var relatedLO = src.ApplicableLOs
                            .First(lo => lo.LearningObjectSummaryId == relatedHint.LearningObjectSummaryId);
                        hintDto.LearningObject = _mapper.Map<LearningObjectDTO>(relatedLO);
                    }
                });
            }));
        }

        [HttpPost("question")]
        public ActionResult<List<AnswerEvaluationDTO>> SubmitQuestionAnswers([FromBody] QuestionSubmissionDTO submission)
        {
            var evaluation = _submissionService.EvaluateAnswers(_mapper.Map<QuestionSubmission>(submission));
            if (evaluation == null) return NotFound();
            return Ok(_mapper.Map<List<AnswerEvaluationDTO>>(evaluation));
        }

        [HttpPost("arrange-task")]
        public ActionResult<List<ArrangeTaskContainerEvaluationDTO>> SubmitArrangeTask([FromBody] ArrangeTaskSubmissionDTO submission)
        {
            var evaluation = _submissionService.EvaluateArrangeTask(_mapper.Map<ArrangeTaskSubmission>(submission));
            if (evaluation == null) return NotFound();
            return Ok(_mapper.Map<List<ArrangeTaskContainerEvaluationDTO>>(evaluation));
        }
    }
}
