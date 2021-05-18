using Microsoft.EntityFrameworkCore;
using SmartTutor.Database;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.QualityAnalysis.Repository
{
    public class AdviceDatabaseRepository: IAdviceRepository
    {
        private readonly SmartTutorContext _dbContext;

        public AdviceDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<IssueAdvice> GetAdvice(List<string> issueTypes)
        {
            return _dbContext.Advice
                .Where(a => issueTypes.Contains(a.IssueType))
                .Include(a => a.Summaries).ToList();
        }
    }
}
