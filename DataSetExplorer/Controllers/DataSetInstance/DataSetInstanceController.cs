using DataSetExplorer.Controllers.DataSetInstance.DTOs;
using DataSetExplorer.DataSetBuilder;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Controllers.DataSetInstance
{
    [Route("api/dataset-instance/")]
    [ApiController]
    public class DataSetInstanceController : ControllerBase
    {
        private readonly IDataSetInstanceService _dataSetInstanceService;

        public DataSetInstanceController(IDataSetInstanceService dataSetInstanceService)
        {
            _dataSetInstanceService = dataSetInstanceService;
        }

        [HttpPost]
        [Route("annotation")]
        public IActionResult AddDataSetAnnotation([FromBody] DataSetAnnotationDTO annotation)
        {
            try 
            {
                var authHeader = HttpContext.Request.Headers["Authorization"];
                int annotatorId = Int32.Parse(authHeader);
                var result = _dataSetInstanceService.AddDataSetAnnotation(annotation, annotatorId);
                return Ok(new { message = result.Value });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
