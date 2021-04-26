﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.DTOs.Content;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.Controllers
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
            return lectures.Select(l => _mapper.Map<LectureDTO>(l)).ToList();
        }
    }
}