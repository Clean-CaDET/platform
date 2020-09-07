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

        [HttpGet]
        public CaDETProject Get()
        {
            return _repositoryService.BuildModel(null);
        }
    }
}
