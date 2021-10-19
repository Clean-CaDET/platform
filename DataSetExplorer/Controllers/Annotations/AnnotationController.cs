using AutoMapper;
using DataSetExplorer.Controllers.Annotations.DTOs;
using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DataSetExplorer.Controllers.Annotations
{
    [Route("api/annotation/")]
    [ApiController]
    public class AnnotationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IDataSetAnnotationService _dataSetAnnotationService;
        private readonly IDataSetAnalysisService _dataSetAnalysisService;

        public AnnotationController(IMapper mapper, IConfiguration configuration, IDataSetAnnotationService dataSetAnnotationService, IDataSetAnalysisService dataSetAnalysisService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _dataSetAnnotationService = dataSetAnnotationService;
            _dataSetAnalysisService = dataSetAnalysisService;
        }

        [HttpGet]
        [Route("available-code-smells")]
        public IActionResult GetAllCodeSmells()
        {
            return Ok(_configuration.GetSection("Annotating:AvailableCodeSmells").Get<IDictionary<string, string[]>>());
        }

        [HttpGet]
        [Route("available-metrics")]
        public IActionResult GetAllMetrics()
        {
            return Ok(_configuration.GetSection("Annotating:AvailableMetrics").Get<IDictionary<string, string[]>>());
        }

        [HttpGet]
        [Route("available-heuristics")]
        public IActionResult GetAllAvailableHeuristics()
        {
            return Ok(_configuration.GetSection("Annotating:AvailableHeuristics").Get<IDictionary<string, string[]>>());
        }

        [HttpPost]
        public IActionResult AddDataSetAnnotation([FromBody] AnnotationDTO annotation)
        {
            try 
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                annotation.AnnotatorId = Int32.Parse(authHeader);
                var result = _dataSetAnnotationService.AddDataSetAnnotation(_mapper.Map<Annotation>(annotation), annotation.InstanceId, annotation.AnnotatorId);
                if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
                return Ok(result.Value);
            }
            catch (Exception e) when (e is FormatException || e is InvalidOperationException || e is ArgumentException)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult UpdateAnnotation([FromRoute] int id, [FromBody] AnnotationDTO annotation)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                annotation.AnnotatorId = Int32.Parse(authHeader);
                var result = _dataSetAnnotationService.UpdateAnnotation(_mapper.Map<Annotation>(annotation), id, annotation.AnnotatorId);
                if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
                return Ok(result.Value);
            }
            catch (Exception e) when (e is FormatException || e is ArgumentException)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet]
        [Route("requiring-additional-annotation")]
        public IActionResult FindInstancesRequiringAdditionalAnnotation([FromQuery(Name = "projectIds")] string projectIds)
        {
            return FindInstances(projectIds, _dataSetAnalysisService.FindInstancesRequiringAdditionalAnnotation);
        }

        [HttpGet]
        [Route("disagreeing-annotations")]
        public IActionResult FindInstancesWithAllDisagreeingAnnotations([FromQuery(Name = "projectIds")] string projectIds)
        {
            return FindInstances(projectIds, _dataSetAnalysisService.FindInstancesWithAllDisagreeingAnnotations);
        }

        private IActionResult FindInstances(string projectIds, Func<IEnumerable<int>, Result<List<SmellCandidateInstances>>> findInstancesMethod)
        {
            try
            {
                if (projectIds == null) return BadRequest(new { message = "Missing project ids!" });
                var ids = new List<int>();
                foreach (var id in projectIds.Split(',')) ids.Add(Int32.Parse(id));
                var result = findInstancesMethod(ids);
                if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
                return Ok(result.Value);
            }
            catch (FormatException)
            {
                return BadRequest(new { message = "Project ids must be numbers!" });
            }
        }
    }
}
