using AutoMapper;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.TraineeModel;
using SmartTutor.Controllers.DTOs.Lecture;

namespace SmartTutor.Controllers.Mappers
{
    public class LectureProfile : Profile
    {
        public LectureProfile()
        {
            CreateMap<Lecture, LectureDTO>();
            CreateMap<NodeProgress, KnowledgeNodeProgressDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Node.Id))
                .ForMember(dest => dest.LearningObjective, opt => opt.MapFrom(src => src.Node.LearningObjective));

            CreateMap<LearningObject, LearningObjectDTO>().IncludeAllDerived();
            CreateMap<Text, TextDTO>();
            CreateMap<Image, ImageDTO>();
            CreateMap<Video, VideoDTO>();
        }
    }
}
