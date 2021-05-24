using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.Controllers.Progress.DTOs.SubmissionEvaluation;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Submissions;
using System;
using System.Collections.Generic;
using SmartTutor.ProgressModel.Exceptions;

namespace SmartTutor.Controllers.Progress
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
        public ActionResult<ChallengeEvaluationDTO> SubmitChallenge(
            [FromBody] ChallengeSubmissionDTO challengeSubmission)
        {
            ChallengeEvaluation challengeEvaluation;
            try
            {
                challengeEvaluation =
                    _submissionService.EvaluateChallenge(_mapper.Map<ChallengeSubmission>(challengeSubmission));
            }
            catch (LearnerNotEnrolledInCourse e)
            {
                return Forbid(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }

            if (challengeEvaluation == null) return NotFound();
            return Ok(_mapper.Map<ChallengeEvaluationDTO>(challengeEvaluation));
        }

        [HttpPost("question")]
        public ActionResult<List<AnswerEvaluationDTO>> SubmitQuestionAnswers(
            [FromBody] QuestionSubmissionDTO submission)
        {
            try
            {
                var evaluation = _submissionService.EvaluateAnswers(_mapper.Map<QuestionSubmission>(submission));
                if (evaluation == null) return NotFound();
                return Ok(_mapper.Map<List<AnswerEvaluationDTO>>(evaluation));
            }
            catch (LearnerNotEnrolledInCourse e)
            {
                return Forbid(e.Message);
            }
        }

        [HttpPost("arrange-task")]
        public ActionResult<List<ArrangeTaskContainerEvaluationDTO>> SubmitArrangeTask(
            [FromBody] ArrangeTaskSubmissionDTO submission)
        {
            var evaluation = _submissionService.EvaluateArrangeTask(_mapper.Map<ArrangeTaskSubmission>(submission));
            if (evaluation == null) return NotFound();
            return Ok(_mapper.Map<List<ArrangeTaskContainerEvaluationDTO>>(evaluation));
        }
    }
}