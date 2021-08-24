using AutoMapper;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.Controllers.Content.DTOs;
using System.Linq;

namespace SmartTutor.Controllers.Content.Mappers
{
    public class ContentProfile : Profile
    {
        public ContentProfile()
        {
            CreateMap<Lecture, LectureDTO>()
                .ForMember(dest => dest.KnowledgeNodeIds,
                    opt => opt.MapFrom(src => src.KnowledgeNodes.Select(n => n.Id)));

            CreateMap<LearningObject, LearningObjectDTO>().IncludeAllDerived();
            CreateMap<LearningObjectDTO, LearningObject>().IncludeAllDerived();

            CreateMap<TextDTO, Text>();
            CreateMap<VideoDTO, Video>();
            CreateMap<ImageDTO, Image>();
            CreateMap<ChallengeDTO, Challenge>();
            CreateMap<QuestionDTO, Question>();

            CreateMap<Text, TextDTO>();
            CreateMap<Image, ImageDTO>();
            CreateMap<Video, VideoDTO>();
            CreateMap<Challenge, ChallengeDTO>();

            CreateMap<Question, QuestionDTO>();
            CreateMap<QuestionAnswer, QuestionAnswerDTO>();

            CreateMap<ArrangeTask, ArrangeTaskDTO>()
                .ForMember(dest => dest.UnarrangedElements,
                    opt => opt.MapFrom(src => src.Containers.SelectMany(c => c.Elements).ToList()));
            CreateMap<ArrangeTaskContainer, ArrangeTaskContainerDTO>();
            CreateMap<ArrangeTaskElement, ArrangeTaskElementDTO>();
            CreateMap<KnowledgeNode, KnowledgeNodeDto>();
            CreateMap<KnowledgeNodeDto, KnowledgeNode>();
            CreateMap<LearningObjectSummary, LearningObjectSummaryDto>();
            CreateMap<LearningObjectSummaryDto, LearningObjectSummary>();
        }
    }
}