using AutoMapper;
using SmartTutor.Controllers.DTOs.QualityAnalysis;
using SmartTutor.QualityAnalysis;
using System.Linq;

namespace SmartTutor.Controllers.Mappers
{
    public class QualityAnalysisProfile: Profile
    {
        public QualityAnalysisProfile()
        {
            CreateMap<CodeSubmissionDTO, CodeSubmission>();
            CreateMap<CodeEvaluation, CodeEvaluationDTO>()
                .ForMember(dest => dest.CodeSnippetIssueAdvice, opt => opt.MapFrom(src => src.GetIssueAdvice()));
            CreateMap<IssueAdvice, IssueAdviceDTO>()
                .ForMember(dest => dest.SummaryIds, opt => opt.MapFrom(src => src.Summaries.Select(s => s.Id)));
        }
    }
}
