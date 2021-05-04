using SmartTutor.Database;

namespace SmartTutor.ProgressModel.Submissions.Repository
{
    public class SubmissionDatabaseRepository : ISubmissionRepository
    {
        private readonly SmartTutorContext _dbContext;

        public SubmissionDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
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

        public string GetWorkspacePath(int learnerId)
        {
            return "../../../../SmellDetectorTests"; //TODO: Workspace as a value object containing creation date and path (based on basePath from configuration)
        }
    }
}