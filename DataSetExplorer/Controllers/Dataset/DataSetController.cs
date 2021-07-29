using DataSetExplorer.Controllers.Dataset.DTOs;
using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
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
        private readonly string _cloneGitPath = "../../ClonedProjects/";

        private readonly IDataSetCreationService _dataSetCreationService;

        public DataSetController(IDataSetCreationService creationService)
        {
            _dataSetCreationService = creationService;
        }

        [HttpPost]
        public IActionResult CreateDataSet([FromBody] List<ProjectDTO> projects)
        {
            var dataSets = new List<DataSet>();
            foreach (var project in projects)
            {
                var result = _dataSetCreationService.CreateDataSetInDatabase(_cloneGitPath, project.Name, project.Url);
                dataSets.Add(result.Value);
            }
            return this.StatusCode(206, dataSets);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult IsDataSetCreated(int id)
        {
            var result = _dataSetCreationService.GetDataSetIfCreated(id);
            if (result.IsFailed)
            {
                var resultsMessage = result.Reasons[0].Message;
                if (resultsMessage.Contains("not yet created")) return NoContent();
                return BadRequest(resultsMessage);
            }

            return Ok(result.Value);
        }
    }
}
