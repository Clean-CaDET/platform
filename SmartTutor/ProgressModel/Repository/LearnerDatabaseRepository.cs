using Microsoft.EntityFrameworkCore;
using SmartTutor.Database;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.ProgressModel.Submissions;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ProgressModel.Repository
{
    public class LearnerDatabaseRepository : ILearnerRepository
    {
        private readonly SmartTutorContext _dbContext;

        public LearnerDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<NodeProgress> GetActivatedNodes(int learnerId)
        {
            throw new System.NotImplementedException();
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

        public Learner GetById(int learnerId)
        {
            return _dbContext.Learners.Find(learnerId);
        }
        public Learner GetByIndex(string index)
        {
            return _dbContext.Learners.AsNoTracking().FirstOrDefault(trainee => trainee.StudentIndex.Equals(index));
        }
        public void Save(Learner learner)
        {
            _dbContext.Learners.Add(learner);
            _dbContext.SaveChanges();
        }
        public void Update(Learner learner)
        {
            _dbContext.Learners.Update(learner);
            _dbContext.SaveChanges();
        }
    }
}