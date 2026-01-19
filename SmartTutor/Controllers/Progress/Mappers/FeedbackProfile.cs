using AutoMapper;
using SmartTutor.Controllers.Progress.DTOs.Feedback;
using SmartTutor.ProgressModel.Feedback;

namespace SmartTutor.Controllers.Progress.Mappers
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<LearningObjectFeedbackDTO, LearningObjectFeedback>();
            CreateMap<LearningObjectFeedback, LearningObjectFeedbackDTO>();
        }
    }
}