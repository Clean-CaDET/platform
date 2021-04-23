using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.DTOs.Feedback;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Feedback;

namespace SmartTutor.Controllers
{
    [Route("api/feedback/")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IMapper mapper, IFeedbackService feedbackService)
        {
            _mapper = mapper;
            _feedbackService = feedbackService;
        }

        [HttpPost("submit")]
        public ActionResult SubmitFeedback([FromBody] LearningObjectFeedbackDTO feedback)
        {
            _feedbackService.SubmitFeedback(_mapper.Map<LearningObjectFeedback>(feedback));
            return Ok();
        }
    }
}