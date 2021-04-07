using AutoMapper;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.Controllers.DTOs.Challenge;

namespace SmartTutor.Controllers.Mappers
{
    public class ChallengeProfile : Profile
    {
        public ChallengeProfile()
        {
            CreateMap<ChallengeEvaluation, ChallengeEvaluationDTO>()
                .ForMember(dest => dest.ApplicableHints, opt => opt.MapFrom(src => src.ApplicableHints.GetHints()));
            CreateMap<ChallengeHint, ChallengeHintDTO>();
        }
    }
}
