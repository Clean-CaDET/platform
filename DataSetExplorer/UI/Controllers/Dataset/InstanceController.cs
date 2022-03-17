using DataSetExplorer.Core.DataSets;
using Microsoft.AspNetCore.Mvc;

namespace DataSetExplorer.UI.Controllers.Dataset
{
    [Route("api/instances/")]
    [ApiController]
    public class InstanceController : ControllerBase
    {
        private readonly IInstanceService _instanceService;

        public InstanceController(IInstanceService instanceService)
        {
            _instanceService = instanceService;
        }

        [HttpGet]
        [Route("{id}/related-instances")]
        public IActionResult GetInstanceWithRelatedInstances([FromRoute] int id)
        {
            var result = _instanceService.GetInstanceWithRelatedInstances(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("{id}/annotations")]
        public IActionResult GetInstanceWithAnnotations([FromRoute] int id)
        {
            var result = _instanceService.GetInstanceWithAnnotations(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}