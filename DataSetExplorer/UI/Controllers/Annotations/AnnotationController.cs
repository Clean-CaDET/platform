using System;
using System.Collections.Generic;
using AutoMapper;
using DataSetExplorer.Core.Annotations;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Annotations.DTOs;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DataSetExplorer.UI.Controllers.Annotations
{
    [Route("api/")]
    [ApiController]
    public class AnnotationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAnnotationService _annotationService;
        private readonly IDataSetAnalysisService _dataSetAnalysisService;

        public AnnotationController(IMapper mapper, IConfiguration configuration, IAnnotationService annotationService, IDataSetAnalysisService dataSetAnalysisService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _annotationService = annotationService;
            _dataSetAnalysisService = dataSetAnalysisService;
        }

        [HttpGet]
        [Route("annotation/available-code-smells")]
        public IActionResult GetAllCodeSmells()
        {
            return Ok(_configuration.GetSection("Annotating:AvailableCodeSmells").Get<IDictionary<string, string[]>>());
        }

        [HttpGet]
        [Route("annotation/available-metrics")]
        public IActionResult GetAllMetrics()
        {
            return Ok(_configuration.GetSection("Annotating:AvailableMetrics").Get<IDictionary<string, string[]>>());
        }

        [HttpGet]
        [Route("annotation/available-heuristics")]
        public IActionResult GetAllAvailableHeuristics()
        {
            return Ok(_configuration.GetSection("Annotating:AvailableHeuristics").Get<IDictionary<string, string[]>>());
        }

        [HttpPost]
        [Route("annotation/")]
        public IActionResult AddDataSetAnnotation([FromBody] AnnotationDTO annotation)
        {
            try 
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                annotation.AnnotatorId = Int32.Parse(authHeader);
                var result = _annotationService.AddAnnotation(_mapper.Map<Annotation>(annotation), annotation.InstanceId, annotation.AnnotatorId);
                if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
                return Ok(result.Value);
            }
            catch (Exception e) when (e is FormatException || e is InvalidOperationException || e is ArgumentException)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut]
        [Route("annotation/update/{id}")]
        public IActionResult UpdateAnnotation([FromRoute] int id, [FromBody] AnnotationDTO annotation)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                annotation.AnnotatorId = Int32.Parse(authHeader);
                var result = _annotationService.UpdateAnnotation(_mapper.Map<Annotation>(annotation), id, annotation.AnnotatorId);
                if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
                return Ok(result.Value);
            }
            catch (Exception e) when (e is FormatException || e is ArgumentException)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        
        [HttpGet]
        [Route("annotation/requiring-additional-annotation/{id}")]
        public IActionResult FindInstancesRequiringAdditionalAnnotation([FromRoute] int id)
        {
            return FindInstances(id, _dataSetAnalysisService.FindInstancesRequiringAdditionalAnnotation);
        }

        [HttpGet]
        [Route("annotation/disagreeing-annotations/{id}")]
        public IActionResult FindInstancesWithAllDisagreeingAnnotations([FromRoute] int id)
        {
            return FindInstances(id, _dataSetAnalysisService.FindInstancesWithAllDisagreeingAnnotations);
        }

        private IActionResult FindInstances(int projectId, Func<int, Result<List<SmellCandidateInstances>>> searchCriteria)
        {
            var result = searchCriteria(projectId);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("annotation/instances/{id}")]
        public IActionResult GetInstanceWithRelatedInstances([FromRoute] int id)
        {
            var result = _annotationService.GetInstanceWithRelatedInstances(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("instances/{id}/annotations")]
        public IActionResult GetInstanceWithAnnotations([FromRoute] int id)
        {
            var result = _annotationService.GetInstanceWithAnnotations(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}
