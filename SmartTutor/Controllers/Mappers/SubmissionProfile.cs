using AutoMapper;
using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.Controllers.DTOs.Content;
using SmartTutor.Controllers.DTOs.SubmissionEvaluation;
using SmartTutor.ProgressModel.Submissions;
using System.Linq;

namespace SmartTutor.Controllers.Mappers
{
    public class SubmissionProfile : Profile
    {
        public SubmissionProfile()
        {
            CreateMap<ChallengeEvaluation, ChallengeEvaluationDTO>()
                .ForMember(dest => dest.ApplicableHints, opt => opt.MapFrom(src => src.ApplicableHints.GetHints()));
            CreateMap<ChallengeHint, ChallengeHintDTO>();
            CreateMap<ChallengeSubmissionDTO, ChallengeSubmission>();

            CreateMap<QuestionSubmissionDTO, QuestionSubmission>()
                .ForMember(dest => dest.SubmittedAnswerIds, opt => opt.MapFrom(src => src.Answers.Select(a => a.Id)));
            CreateMap<AnswerEvaluation, AnswerEvaluationDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FullAnswer.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.FullAnswer.Text))
                .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.FullAnswer.Feedback));

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
