using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.Content.DTOs;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.Lectures;

namespace SmartTutor.Controllers.Content
{
    [Route("api/content/")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContentService _contentService;

        public ContentController(IMapper mapper, IContentService contentService)
        {
            _mapper = mapper;
            _contentService = contentService;
        }

        [HttpGet("lectures")]
        public ActionResult<List<LectureDTO>> GetLectures()
        {
            var lectures = _contentService.GetLectures();
            return Ok(lectures.Select(l => _mapper.Map<LectureDTO>(l)).ToList());
        }

        [HttpPost("knowledgeNode")]
        public ActionResult<string> CreateKnowledgeNode([FromBody] KnowledgeNodeDto dto)
        {
            _contentService.CreateKnowledgeNode(_mapper.Map<KnowledgeNode>(dto));
            return Ok("Knowledge node is created!");
        }

        [HttpPost("learningObjectSummary")]
        public ActionResult<string> CreateLearningObjectSummary([FromBody] LearningObjectSummaryDto dto)
        {
            _contentService.CreateLearningObjectSummary(_mapper.Map<LearningObjectSummary>(dto));
            return Ok("Learning object summary is created!");
        }

        [HttpGet("lectureNodes/{lectureId}")]
        public ActionResult<List<KnowledgeNodeDto>> GetKnowledgeNodesByLecture([FromRoute] int lectureId)
        {
            var nodes = _contentService.GetKnowledgeNodesByLecture(lectureId);
            return Ok(_mapper.Map<List<KnowledgeNodeDto>>(nodes));
        }

        [HttpGet("learningObjectSummariesByNode/{nodeId}")]
        public ActionResult<List<LearningObjectSummaryDto>> GetLearningObjectSummariesByNode([FromRoute] int nodeId)
        {
            var learningObjectSummaries = _contentService.GetLearningObjectSummariesByNode(nodeId);
            return Ok(_mapper.Map<List<LearningObjectSummaryDto>>(learningObjectSummaries));
        }

        [HttpGet("learningObjectsBySummaries/{losId}")]
        public ActionResult<List<LearningObjectSummaryDto>> GetLearningObjectsByLearningObjectSummary(
            [FromRoute] int losId)
        {
            var learningObjects = _contentService.GetLearningObjectsByLearningObjectSummary(losId);
            return Ok(_mapper.Map<List<LearningObjectDTO>>(learningObjects));
        }
    }
}