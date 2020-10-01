using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("parse/class")]
        public CaDETClassDTO GetBasicClassMetrics([FromBody] string classCode)
        {
            var retVal = new CaDETClassDTO(_repositoryService.BuildClassModel(classCode));
            return retVal;
        }
    }
}
