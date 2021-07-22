using DataSetExplorer.Controllers.Dataset.DTOs;
using DataSetExplorer.DataSetBuilder.Model;
using Microsoft.AspNetCore.Http;
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

        public DataSetController(IDataSetCreationService service)
        {
            _dataSetCreationService = service;
        }

        [HttpPost]
        public IActionResult CreateDataSet([FromBody] List<ProjectDTO> projects)
        {
            var dataSets = new List<DataSet>();
            foreach (var project in projects)
            {
                dataSets.Add(_dataSetCreationService.CreateDataSet(_cloneGitPath, project.Name, project.Url));
            }
            
            return Ok(dataSets);
        }
    }
}
