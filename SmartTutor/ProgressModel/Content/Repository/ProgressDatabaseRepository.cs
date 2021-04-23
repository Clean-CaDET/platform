using SmartTutor.Database;
using System.Linq;

namespace SmartTutor.ProgressModel.Content.Repository
{
    public class ProgressDatabaseRepository : IProgressRepository
    {
        private readonly SmartTutorContext _dbContext;

        public ProgressDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveNodeProgress(NodeProgress nodeProgress)
        {
            _dbContext.NodeProgresses.Add(nodeProgress);
            _dbContext.SaveChanges();
        }
        public NodeProgress GetNodeProgressForLearner(int learnerId, int nodeId)
        {
            return _dbContext.NodeProgresses.FirstOrDefault(nodeProgress =>
                nodeProgress.Learner.Id == learnerId && nodeProgress.Node.Id == nodeId);
        }
    }
}