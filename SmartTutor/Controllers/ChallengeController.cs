using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.DTOs;

namespace SmartTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        [HttpPost("check")]
        public ChallengeCheckResponse CheckChallengeCompletion([FromBody] ChallengeCheckRequest checkRequest)
        {
            var text = checkRequest.ChallengeId == 1 ? "Fail." : "Success.";
            return new ChallengeCheckResponse(text);
        }
    }
}
