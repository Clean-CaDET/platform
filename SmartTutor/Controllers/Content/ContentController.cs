using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.Content.DTOs;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.DTOs;
using SmartTutor.ContentModel.Exceptions;

namespace SmartTutor.Controllers.Content
{
    [Route("api/lectures/")]
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

        [HttpPost]
        public ActionResult<string> CreateCourse([FromBody] CreateCourseDto dto)
        {
            try
            {
                _contentService.CreateCourse(dto);
                return Ok("Course is created!");
            }
            catch (Exception e)
            {
                return Problem("Sorry, there has been an problem on server side.");
            }
        }

        [HttpPost]
        public ActionResult<string> CreateLecture([FromBody] CreateLectureDto dto)
        {
            try
            {
                _contentService.CreateLecture(dto);
                return Ok("Lecture is created!");
            }
            catch (Exception e)
            {
                return Problem("Sorry, there has been an problem on server side.");
            }
        }
    }
}
