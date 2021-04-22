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

        public List<NodeProgress> GetActivatedNodes(int traineeId)
        {
            throw new System.NotImplementedException();
        }
        public void SaveNodeProgress(NodeProgress nodeProgress)
        {
            _dbContext.NodeProgresses.Add(nodeProgress);
            _dbContext.SaveChanges();
        }
        public NodeProgress GetNodeProgressForTrainee(int traineeId, int nodeId)
        {
            return _dbContext.NodeProgresses.FirstOrDefault(nodeProgress =>
                nodeProgress.Learner.Id == traineeId && nodeProgress.Node.Id == nodeId);
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

        public Learner GetTraineeById(int traineeId)
        {
            return _dbContext.Trainees.Find(traineeId);
        }
        public Learner GetLearnerByIndex(string index)
        {
            return _dbContext.Trainees.AsNoTracking().FirstOrDefault(trainee => trainee.StudentIndex.Equals(index));
        }
        public void SaveTrainee(Learner learner)
        {
            _dbContext.Trainees.Add(learner);
            _dbContext.SaveChanges();
        }
        public void UpdateTrainee(Learner learner)
        {
            _dbContext.Trainees.Update(learner);
            _dbContext.SaveChanges();
        }
    }
}