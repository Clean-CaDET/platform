using AutoMapper;
using SmartTutor.Controllers.DTOs.Learner;
using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.Controllers.Mappers
{
    public class LearnerProfile : Profile
    {
        public LearnerProfile()
        {
            CreateMap<LearnerDTO, Learner>();
            CreateMap<Learner, LearnerDTO>();
        }
    }
}