using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.DTOs.Lecture;

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
            if (nodes == null) NotFound();
            return Ok(_mapper.Map<KnowledgeNodeProgressDTO>(nodes));
        }

        [HttpPost("{nodeId}/content/{questionId}")]
        public ActionResult<List<AnswerEvaluationDTO>> SubmitQuestionAnswers(int nodeId, int questionId, [FromBody] List<QuestionAnswerDTO> submittedAnswers)
        {
            //TODO: NodeId will be useful for ProgressModel, we should see if this is KNid or KNProgressId
            var evaluation = _contentService.EvaluateAnswers(questionId, submittedAnswers.Select(a => a.Id).ToList());
            if (evaluation == null) NotFound();
            return Ok(_mapper.Map<List<AnswerEvaluationDTO>>(evaluation));
        }
    }
}
