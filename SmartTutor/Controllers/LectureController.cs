using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.DTOs.Lecture;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.Controllers
{
    [Route("api/lectures/")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContentService _contentService;

        public LectureController(IMapper mapper, IContentService contentService)
        {
            _mapper = mapper;
            _contentService = contentService;
        }

        [HttpGet]
        public ActionResult<List<LectureDTO>> GetLectures()
        {
            var lectures = _contentService.GetLectures();
            return lectures.Select(l => _mapper.Map<LectureDTO>(l)).ToList();
        }

        [HttpGet("{lectureId}")]
        public ActionResult<List<KnowledgeNodeProgressDTO>> GetLectureNodes(int lectureId)
        {
            //TODO: Extract and send trainee ID.
            var nodes = _contentService.GetKnowledgeNodes(lectureId, null);
            if (nodes == null) NotFound();
            return Ok(_mapper.Map<List<KnowledgeNodeProgressDTO>>(nodes));
        }
    }
}
