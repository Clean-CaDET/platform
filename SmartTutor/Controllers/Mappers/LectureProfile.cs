using AutoMapper;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.LectureModel.LearningObjects;
using SmartTutor.Controllers.DTOs.Lecture;

namespace SmartTutor.Controllers.Mappers
{
    public class LectureProfile : Profile
    {
        public LectureProfile()
        {
            CreateMap<Lecture, LectureDTO>();
            CreateMap<KnowledgeNode, KnowledgeNodeDTO>();

            CreateMap<LearningObject, LearningObjectDTO>().IncludeAllDerived();
            CreateMap<LearningText, LearningTextDTO>();
            CreateMap<LearningImage, LearningImageDTO>();
            CreateMap<LearningVideo, LearningVideoDTO>();
        }
    }
}
