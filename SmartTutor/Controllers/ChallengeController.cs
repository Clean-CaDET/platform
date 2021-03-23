using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.DTOs.Challenge;

namespace SmartTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        [HttpPost("check")]
        public ChallengeCheckResponseDTO CheckChallengeCompletion([FromBody] ChallengeCheckRequestDTO checkRequestDto)
        {
            var text = checkRequestDto.ChallengeId == 1 ? "Fail." : "Success.";
            return new ChallengeCheckResponseDTO(text);
        }
    }
}
