using AutoMapper;
using SmartTutor.Controllers.Learners.DTOs;

namespace SmartTutor.Controllers.Learners
{
    public class LearnerProfile : Profile
    {
        public LearnerProfile()
        {
            CreateMap<LearnerDTO, LearnerModel.Learners.Learner>();
            CreateMap<LearnerModel.Learners.Learner, LearnerDTO>();
        }
    }
}