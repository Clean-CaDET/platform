using SmartTutor.Database;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.ProgressModel.Repository
{
    public class TraineeDatabaseRepository : ITraineeRepository
    {
        private readonly SmartTutorContext _dbContext;

        public TraineeDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<NodeProgress> GetActivatedNodes(int traineeId)
        {
            throw new System.NotImplementedException();
        }

        public Trainee GetTraineeById(int traineeId)
        {
            return _dbContext.Trainees.Find(traineeId);
        }

        public void SaveNodeProgress(NodeProgress nodeProgress)
        {
            _dbContext.NodeProgresses.Add(nodeProgress);
            _dbContext.SaveChanges();
        }

        public NodeProgress GetNodeProgressForTrainee(int traineeId, int nodeId)
        {
            return _dbContext.NodeProgresses.FirstOrDefault(nodeProgress =>
                nodeProgress.Trainee.Id == traineeId && nodeProgress.Node.Id == nodeId);
        }

        public void SaveChallengeSubmission(ChallengeSubmission submission)
        {
            _dbContext.ChallengeSubmission.Add(submission);
            _dbContext.SaveChanges();
        }
    }
}