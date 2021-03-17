using Microsoft.AspNetCore.Mvc;

namespace SmartTutor.Controllers
{
    [Route("api/smarttutor")]
    [ApiController]
    public class SmarttutorController : ControllerBase
    {
        public SmarttutorController()
        {
      
        }

        [HttpPost("education/class")]
        public string GetClassMetricsWithCohesionFeedback([FromBody] string classCode)
        {
            return "SUCC";
        }
    }
}
