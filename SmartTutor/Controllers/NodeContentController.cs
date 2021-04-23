using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.DTOs.Lecture;
using SmartTutor.ProgressModel;
using System.Collections.Generic;

namespace SmartTutor.Controllers
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
            //TODO: Extract and send trainee ID.
            var nodes = _progressService.GetKnowledgeNodes(lectureId, null);
            if (nodes == null) return NotFound();
            return Ok(_mapper.Map<List<KnowledgeNodeProgressDTO>>(nodes));
        }

        [HttpGet("content/{nodeId}")]
        public ActionResult<KnowledgeNodeProgressDTO> GetNodeContent(int nodeId)
        {
            var nodes = _progressService.GetNodeContent(nodeId, null);
            if (nodes == null) return NotFound();
            return Ok(_mapper.Map<KnowledgeNodeProgressDTO>(nodes));
        }
    }
}
