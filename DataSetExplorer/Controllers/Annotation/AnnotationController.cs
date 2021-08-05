using AutoMapper;
using DataSetExplorer.Controllers.Annotation.DTOs;
using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Controllers.Annotation
{
    [Route("api/annotation/")]
    [ApiController]
    public class AnnotationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDataSetAnnotationService _dataSetAnnotationService;
        private readonly IDataSetAnalysisService _dataSetAnalysisService;

        public AnnotationController(IMapper mapper, IDataSetAnnotationService dataSetAnnotationService, IDataSetAnalysisService dataSetAnalysisService)
        {
            _mapper = mapper;
            _dataSetAnnotationService = dataSetAnnotationService;
            _dataSetAnalysisService = dataSetAnalysisService;
        }

        [HttpPost]
        public IActionResult AddDataSetAnnotation([FromBody] DataSetAnnotationDTO annotation)
        {
            try 
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                annotation.AnnotatorId = Int32.Parse(authHeader);
                var dataSetAnnotation = _mapper.Map<DataSetAnnotation>(annotation);
                var result = _dataSetAnnotationService.AddDataSetAnnotation(dataSetAnnotation, annotation.DataSetInstanceId, annotation.AnnotatorId);
                if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
                return Ok(new { message = result.Value });
            }
            catch (FormatException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet]
        [Route("requiring-additional-annotation/{dataSetId}")]
        public IActionResult FindInstancesRequiringAdditionalAnnotation(int dataSetId)
        {
            var result = _dataSetAnalysisService.FindInstancesRequiringAdditionalAnnotation(dataSetId);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("disagreeing-annotations/{dataSetId}")]
        public IActionResult FindInstancesWithAllDisagreeingAnnotations(int dataSetId)
        {
            var result = _dataSetAnalysisService.FindInstancesWithAllDisagreeingAnnotations(dataSetId);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}
