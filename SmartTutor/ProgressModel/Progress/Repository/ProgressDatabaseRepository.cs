using SmartTutor.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SmartTutor.ProgressModel.Progress.Repository
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
            return _dbContext.NodeProgresses
                .Where(nodeProgress => nodeProgress.LearnerId == learnerId && nodeProgress.Node.Id == nodeId)
                .Include(np => np.LearningObjects)
                .FirstOrDefault();
        }
    }
}