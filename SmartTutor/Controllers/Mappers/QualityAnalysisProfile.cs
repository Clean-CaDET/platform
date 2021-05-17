using AutoMapper;
using SmartTutor.Controllers.DTOs.QualityAnalysis;
using SmartTutor.QualityAnalysis;

namespace SmartTutor.Controllers.Mappers
{
    public class QualityAnalysisProfile: Profile
    {
        public QualityAnalysisProfile()
        {
            CreateMap<CodeSubmissionDTO, CodeSubmission>();
            CreateMap<CodeEvaluation, CodeEvaluationDTO>()
                .ForMember(dest => dest.CodeSnippetIssueAdvice, opt => opt.MapFrom(src => src.GetIssueAdvice()));
        }
    }
}
