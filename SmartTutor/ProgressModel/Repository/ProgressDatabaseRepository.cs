using Microsoft.EntityFrameworkCore;
using SmartTutor.Database;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.ProgressModel.Submissions;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ProgressModel.Repository
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

        public void SaveChallengeSubmission(ChallengeSubmission submission)
        {
            _dbContext.ChallengeSubmissions.Add(submission);
            _dbContext.SaveChanges();
        }

        public void SaveQuestionSubmission(QuestionSubmission submission)
        {
            _dbContext.QuestionSubmissions.Add(submission);
            _dbContext.SaveChanges();
        }

        public void SaveArrangeTaskSubmission(ArrangeTaskSubmission submission)
        {
            _dbContext.ArrangeTaskSubmissions.Add(submission);
            _dbContext.SaveChanges();
        }
    }
}