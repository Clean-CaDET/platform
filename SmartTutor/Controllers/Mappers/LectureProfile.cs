using AutoMapper;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.ProgressModel;
using SmartTutor.Controllers.DTOs.Lecture;
using System.Linq;

namespace SmartTutor.Controllers.Mappers
{
    public class LectureProfile : Profile
    {
        public LectureProfile()
        {
            CreateMap<Lecture, LectureDTO>()
                .ForMember(dest => dest.KnowledgeNodeIds, opt => opt.MapFrom(src => src.KnowledgeNodes.Select(n => n.Id)));
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

            CreateMap<QuestionSubmissionDTO, QuestionSubmission>()
                .ForMember(dest => dest.submittedAnswerIds, opt => opt.MapFrom(src => src.Answers.Select(a => a.Id)));

            CreateMap<ArrangeTask, ArrangeTaskDTO>()
                .ForMember(dest => dest.UnarrangedElements, opt => opt.MapFrom(src => src.Containers.SelectMany(c => c.Elements).ToList()));
            CreateMap<ArrangeTaskContainer, ArrangeTaskContainerDTO>();
            CreateMap<ArrangeTaskElement, ArrangeTaskElementDTO>();
            
            CreateMap<ArrangeTaskContainerEvaluation, ArrangeTaskContainerEvaluationDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FullAnswer.Id))
                .ForMember(dest => dest.CorrectElements, opt => opt.MapFrom(src => src.FullAnswer.Elements));
            CreateMap<ArrangeTaskSubmissionDTO, ArrangeTaskSubmission>();
            CreateMap<ArrangeTaskContainerDTO, ArrangeTaskContainerSubmission>()
                .ForMember(dest => dest.ElementIds, opt => opt.MapFrom(src => src.Elements.Select(e => e.Id)))
                .ForMember(dest => dest.ContainerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
