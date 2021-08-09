using DataSetExplorer.Controllers.Dataset.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DataSetExplorer.Controllers.Dataset
{
    [Route("api/dataset/")]
    [ApiController]
    public class DataSetController : ControllerBase
    {
        private readonly string _gitClonePath;

        private readonly IDataSetCreationService _dataSetCreationService;

        public DataSetController(IDataSetCreationService creationService, IConfiguration configuration)
        {
            _dataSetCreationService = creationService;
            _gitClonePath = configuration.GetValue<string>("Workspace:GitClonePath");
        }

        [HttpPost]
        public IActionResult CreateDataSet([FromBody] List<ProjectDTO> projects) // TODO: Add dataSetName parameter
        {
            var dataSetProjects = new Dictionary<string, string>();
            foreach (var project in projects)
            {
                dataSetProjects[project.Name] = project.Url;
            }

            // TODO: Send dataSetName parameter to CreateDataSetInDatabase method instead of "Clean CaDET"
            var result = _dataSetCreationService.CreateDataSetInDatabase("Clean CaDET", _gitClonePath, dataSetProjects);
            return Accepted(result.Value);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDataSet(int id)
        {
            var result = _dataSetCreationService.GetDataSet(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}
