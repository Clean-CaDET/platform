using System.Collections.Generic;

namespace SmartTutor.QualityAnalysis
{
    public interface IAdviceRepository
    {
        List<IssueAdvice> GetAdvice(List<string> issueTypes);
    }
}