using AutoMapper;
using SmartTutor.Controllers.DTOs.Learner;
using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.Controllers.Mappers
{
    public class ProgressProfile : Profile
    {
        public ProgressProfile()
        {
            CreateMap<LearnerDTO, Learner>();
            CreateMap<Learner, LearnerDTO>();
        }
    }
}