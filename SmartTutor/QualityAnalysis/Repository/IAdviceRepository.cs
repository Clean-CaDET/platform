using System.Collections.Generic;

namespace SmartTutor.QualityAnalysis.Repository
{
    public interface IAdviceRepository
    {
        List<IssueAdvice> GetAdvice(List<string> issueTypes);
    }
}