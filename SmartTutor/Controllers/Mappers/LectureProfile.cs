using System.Linq;
using AutoMapper;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.ProgressModel;
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
            CreateMap<Challenge, ChallengeDTO>();

            CreateMap<Question, QuestionDTO>();
            CreateMap<QuestionAnswer, QuestionAnswerDTO>();
            CreateMap<AnswerEvaluation, AnswerEvaluationDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FullAnswer.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.FullAnswer.Text))
                .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.FullAnswer.Feedback));

            CreateMap<ArrangeTask, ArrangeTaskDTO>()
                .ForMember(dest => dest.UnarrangedElements, opt => opt.MapFrom(src => src.Containers.SelectMany(c => c.CorrectlyArrangedElements).ToList()));
            CreateMap<ArrangeTaskContainer, ArrangeTaskContainerDTO>();
            CreateMap<ArrangeTaskElement, ArrangeTaskElementDTO>();
        }
    }
}
