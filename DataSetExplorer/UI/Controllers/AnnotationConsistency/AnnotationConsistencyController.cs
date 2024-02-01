using DataSetExplorer.Core.AnnotationConsistency;
using Microsoft.AspNetCore.Mvc;

namespace DataSetExplorer.UI.Controllers.AnnotationConsistency
{
    [Route("api/annotation-consistency/")]
    [ApiController]
    public class AnnotationConsistencyController : ControllerBase
    {
        private readonly IAnnotationConsistencyService _annotationConsistencyService;

        public AnnotationConsistencyController(IAnnotationConsistencyService annotationConsistencyService)
        {
            _annotationConsistencyService = annotationConsistencyService;
        }

        [HttpGet]
        [Route("annotator/{projectId}/{annotatorId}")]
        public IActionResult GetAnnotationConsistencyForAnnotator([FromRoute] int projectId, [FromRoute] int annotatorId)
        {
            var result = _annotationConsistencyService.CheckAnnotationConsistencyForAnnotator(projectId, annotatorId);
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("annotators/{projectId}/{severity}")]
        public IActionResult GetAnnotationConsistencyBetweenAnnotatorsForSeverity([FromRoute] int projectId, [FromRoute] string severity)
        {
            var result = _annotationConsistencyService.CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(projectId, severity);
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("metrics/annotator/{projectId}/{annotatorId}")]
        public IActionResult GetMetricsSignificanceInAnnotationsForAnnotator([FromRoute] int projectId, [FromRoute] int annotatorId)
        {
            var result = _annotationConsistencyService.CheckMetricsSignificanceInAnnotationsForAnnotator(projectId, annotatorId);
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("metrics/annotators/{projectId}/{severity}")]
        public IActionResult GetMetricsSignificanceBetweenAnnotatorsForSeverity([FromRoute] int projectId, [FromRoute] string severity)
        {
            var result = _annotationConsistencyService.CheckMetricsSignificanceBetweenAnnotatorsForSeverity(projectId, severity);
            return Ok(result.Value);
        }
    }
}
