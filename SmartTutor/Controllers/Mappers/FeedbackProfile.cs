using AutoMapper;
using SmartTutor.ContentModel.Feedback;
using SmartTutor.Controllers.DTOs.Feedback;

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