using AutoMapper;
using SmartTutor.Controllers.DTOs.Progress;
using SmartTutor.ProgressModel.Progress;

namespace SmartTutor.Controllers.Mappers
{
    public class ProgressProfile: Profile
    {
        public ProgressProfile()
        {
            CreateMap<NodeProgress, KnowledgeNodeProgressDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Node.Id))
                .ForMember(dest => dest.LearningObjective, opt => opt.MapFrom(src => src.Node.LearningObjective));
        }
    }
}
