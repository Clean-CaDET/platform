using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.Progress.DTOs.Progress;
using SmartTutor.ProgressModel;
using System.Collections.Generic;

namespace SmartTutor.Controllers.Progress
{
    [Route("api/nodes/")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProgressService _progressService;

        public ProgressController(IMapper mapper, IProgressService progressService)
        {
            _mapper = mapper;
            _progressService = progressService;
        }

        [HttpGet("{lectureId}")]
        public ActionResult<List<KnowledgeNodeProgressDTO>> GetLectureNodes(int lectureId)
        {
            //TODO: Extract learner ID so that we can see the Status of each KN.
            var nodes = _progressService.GetKnowledgeNodes(lectureId, null);
            if (nodes == null) return NotFound("Lecture does not contain any nodes.");
            return Ok(_mapper.Map<List<KnowledgeNodeProgressDTO>>(nodes));
        }
        //TODO: The URLs don't follow best practices because the tension between KN and KNProgress. Will need to study this more.
        [HttpGet("content/{nodeId}")]
        public ActionResult<KnowledgeNodeProgressDTO> GetNodeContent(int nodeId)
        {
            //TODO: Extract learner ID so that we can form personalized content.
            var nodes = _progressService.GetNodeContent(nodeId, null);
            if (nodes == null) return NotFound();
            return Ok(_mapper.Map<KnowledgeNodeProgressDTO>(nodes));
        }
    }
}
