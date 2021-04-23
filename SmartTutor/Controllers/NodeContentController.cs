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
            if (nodes == null) return NotFound();
            return Ok(_mapper.Map<KnowledgeNodeProgressDTO>(nodes));
        }
    }
}
