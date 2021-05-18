using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.Progress.DTOs.Feedback;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Feedback;

namespace SmartTutor.Controllers.Progress
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

        [HttpPost]
        public ActionResult SubmitFeedback([FromBody] LearningObjectFeedbackDTO feedback)
        {
            _feedbackService.SubmitFeedback(_mapper.Map<LearningObjectFeedback>(feedback));
            return Ok();
        }
    }
}