using SmartTutor.Database;
using SmartTutor.LearnerModel.Learners.Workspaces;
using System.Linq;

namespace SmartTutor.LearnerModel.Learners.Repository
{
    public class LearnerDatabaseRepository : ILearnerRepository
    {
        private readonly SmartTutorContext _dbContext;

        public LearnerDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Learner GetById(int learnerId)
        {
            return _dbContext.Learners.Find(learnerId);
        }
        public Learner GetByIndex(string index)
        {
            return _dbContext.Learners.FirstOrDefault(learner => learner.StudentIndex.Equals(index));
        }

        public Learner SaveOrUpdate(Learner learner)
        {
            _dbContext.Learners.Attach(learner);
            _dbContext.SaveChanges();
            return learner;
        }

        public void Save(Workspace workspace)
        {
            _dbContext.Workspaces.Attach(workspace);
            _dbContext.SaveChanges();
        }
    }
}
