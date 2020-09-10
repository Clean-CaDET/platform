using Microsoft.AspNetCore.Mvc;
using RepositoryCompiler.CodeParsers.CaDETModel;

namespace RepositoryCompiler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly CodeRepositoryService _repositoryService;

        public RepositoryController(CodeRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        [HttpGet("clone")]
        public string SetupRepository()
        {
            _repositoryService.SetupRepository();
            return "done";
        }

        [HttpGet("parse")]
        public CaDETProject GetCurrentCommitModel()
        {
            return _repositoryService.BuildProjectModel((string)null);
        }
            
        [HttpGet("multiple/{numOfCommits}")]
        public CaDETModel GetMultipleCommitModel(int numOfCommits)
        {
            return _repositoryService.BuildModel(numOfCommits);
        }

        [HttpGet("pull")]
        public bool UpdateRepository()
        {
            return _repositoryService.UpdateRepository();
        }
    }
}
