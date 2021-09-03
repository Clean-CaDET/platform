using AutoMapper;
using DataSetExplorer.Controllers.Annotation.DTOs;
using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DataSetExplorer.Controllers.Annotation
{
    [Route("api/annotation/")]
    [ApiController]
    public class AnnotationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IDataSetAnnotationService _dataSetAnnotationService;
        private readonly IDataSetAnalysisService _dataSetAnalysisService;
        private readonly IAnnotationConsistencyService _annotationConsistencyService;

        public AnnotationController(IMapper mapper, IConfiguration configuration, IDataSetAnnotationService dataSetAnnotationService, IDataSetAnalysisService dataSetAnalysisService, IAnnotationConsistencyService annotationConsistencyService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _dataSetAnnotationService = dataSetAnnotationService;
            _dataSetAnalysisService = dataSetAnalysisService;
            _annotationConsistencyService = annotationConsistencyService;
        }

        [HttpGet]
        [Route("available-code-smells")]
        public IActionResult GetAllCodeSmells()
        {
            return Ok(_configuration.GetSection("Annotating:AvailableCodeSmells").Get<IDictionary<string, string[]>>());
        }

        [HttpGet]
        [Route("available-heuristics")]
        public IActionResult GetAllAvailableHeuristics()
        {
            return Ok(_configuration.GetSection("Annotating:AvailableHeuristics").Get<IDictionary<string, string[]>>());
        }

        [HttpPost]
        public IActionResult AddDataSetAnnotation([FromBody] DataSetAnnotationDTO annotation)
        {
            try 
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                annotation.AnnotatorId = Int32.Parse(authHeader);
                var result = _dataSetAnnotationService.AddDataSetAnnotation(_mapper.Map<DataSetAnnotation>(annotation), annotation.DataSetInstanceId, annotation.AnnotatorId);
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
        public IActionResult UpdateAnnotation([FromRoute] int id, [FromBody] DataSetAnnotationDTO annotation)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                annotation.AnnotatorId = Int32.Parse(authHeader);
                var result = _dataSetAnnotationService.UpdateAnnotation(_mapper.Map<DataSetAnnotation>(annotation), id, annotation.AnnotatorId);
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

        [HttpGet]
        [Route("consistency/annotator/{projectId}/{annotatorId}")]
        public IActionResult GetAnnotationConsistencyForAnnotator([FromRoute] int projectId, [FromRoute] int annotatorId)
        {
            var result = _annotationConsistencyService.CheckAnnotationConsistencyForAnnotator(projectId, annotatorId);
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("consistency/annotators/{projectId}/{severity}")]
        public IActionResult GetAnnotationConsistencyBetweenAnnotatorsForSeverity([FromRoute] int projectId, [FromRoute] int severity)
        {
            var result = _annotationConsistencyService.CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(projectId, severity);
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("consistency/metrics/annotator/{projectId}/{annotatorId}")]
        public IActionResult GetMetricsSignificanceInAnnotationsForAnnotator([FromRoute] int projectId, [FromRoute] int annotatorId)
        {
             var result = _annotationConsistencyService.CheckMetricsSignificanceInAnnotationsForAnnotator(projectId, annotatorId);
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("consistency/metrics/annotators/{projectId}/{severity}")]
        public IActionResult GetMetricsSignificanceBetweenAnnotatorsForSeverity([FromRoute] int projectId, [FromRoute] int severity)
        {
            var result = _annotationConsistencyService.CheckMetricsSignificanceBetweenAnnotatorsForSeverity(projectId, severity);
            return Ok(result.Value);
        }

        private IActionResult FindInstances(string projectIds, Func<IEnumerable<int>, Result<List<DataSetInstance>>> findInstancesMethod)
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
