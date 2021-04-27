using AutoMapper;
using SmartTutor.Controllers.DTOs.Feedback;
using SmartTutor.ProgressModel.Feedback;

namespace SmartTutor.Controllers.Mappers
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