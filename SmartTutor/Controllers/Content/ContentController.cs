using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.Content.DTOs;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.Exceptions;
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

        [HttpGet]
        public ActionResult<List<LectureDTO>> GetLectures()
        {
            var lectures = _contentService.GetLectures();
            return Ok(lectures.Select(l => _mapper.Map<LectureDTO>(l)).ToList());
        }

        [HttpPost("course")]
        public ActionResult<string> CreateCourse([FromBody] CreateCourseDto dto)
        {
            try
            {
                _contentService.CreateCourse(_mapper.Map<Course>(dto.Course), dto.TeacherId);
                return Ok("Course is created!");
            }
            catch (NotEnoughResourcesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("lecture")]
        public ActionResult<string> CreateLecture([FromBody] CreateLectureDto dto)
        {
            try
            {
                _contentService.CreateLecture(_mapper.Map<Lecture>(dto.Lecture), dto.TeacherId);
                return Ok("Lecture is created!");
            }
            catch (NotEnoughResourcesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("lecture/teacher/{id}")]
        public ActionResult<List<CreateLectureDto>> GetLecturesByTeachersId([FromRoute] int id)
        {
            var dtos = _contentService.GetLecturesByTeachersId(id);
            return Ok(dtos);
        }

        [HttpGet("course/teacher/{id}")]
        public ActionResult<List<CreateCourseDto>> GetCoursesByTeachersId([FromRoute] int id)
        {
            var dtos = _contentService.GetCoursesByTeachersId(id);
            return Ok(dtos);
        }
    }
}