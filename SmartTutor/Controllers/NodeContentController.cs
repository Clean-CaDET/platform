using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.Controllers.DTOs.Lecture;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.ProgressModel;

namespace SmartTutor.Controllers
{
    [Route("api/nodes/")]
    [ApiController]
    public class NodeContentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContentService _contentService;

        public NodeContentController(IMapper mapper, IContentService contentService)
        {
            _mapper = mapper;
            _contentService = contentService;
        }

        [HttpGet("{nodeId}/content")]
        public ActionResult<KnowledgeNodeProgressDTO> GetNodeContent(int nodeId)
        {
            var nodes = _contentService.GetNodeContent(nodeId, null);
            if (nodes == null) return NotFound();
            return Ok(_mapper.Map<KnowledgeNodeProgressDTO>(nodes));
        }

        [HttpPost("{nodeId}/content/question")]
        public ActionResult<List<AnswerEvaluationDTO>> SubmitQuestionAnswers(int nodeId, [FromBody] QuestionSubmissionDTO submission)
        {
            //TODO: NodeId will be useful for ProgressModel, we should see if this is KNid or KNProgressId
            var evaluation = _contentService.EvaluateAnswers(_mapper.Map<QuestionSubmission>(submission));
            if (evaluation == null) return NotFound();
            return Ok(_mapper.Map<List<AnswerEvaluationDTO>>(evaluation));
        }

        [HttpPost("{nodeId}/content/arrange-task/{arrangeTaskId}")]
        public ActionResult<List<ArrangeTaskContainerEvaluationDTO>> SubmitArrangeTask(int nodeId, int arrangeTaskId, [FromBody] List<ArrangeTaskContainerDTO> submittedAnswers)
        {
            //TODO: NodeId will be useful for ProgressModel, we should see if this is KNid or KNProgressId
            var evaluation = _contentService.EvaluateArrangeTask(arrangeTaskId, _mapper.Map<List<ArrangeTaskContainer>>(submittedAnswers));
            if (evaluation == null) return NotFound();
            return Ok(_mapper.Map<List<ArrangeTaskContainerEvaluationDTO>>(evaluation));
        }
    }
}
