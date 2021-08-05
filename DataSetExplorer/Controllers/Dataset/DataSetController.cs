using DataSetExplorer.Controllers.Dataset.DTOs;
using DataSetExplorer.DataSetBuilder.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult CreateDataSet([FromBody] List<ProjectDTO> projects)
        {
            var dataSets = new List<DataSet>();
            foreach (var project in projects)
            {
                var result = _dataSetCreationService.CreateDataSetInDatabase(_gitClonePath, project.Name, project.Url);
                dataSets.Add(result.Value);
            }
            return Accepted(dataSets);
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
