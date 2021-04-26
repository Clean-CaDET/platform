using Microsoft.EntityFrameworkCore;
using SmartTutor.Database;
using System.Linq;

namespace SmartTutor.LearnerModel.Learners.Repository
{
    public class LearnerDatabaseRepository: ILearnerRepository
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
            return _dbContext.Learners.AsNoTracking().FirstOrDefault(learner => learner.StudentIndex.Equals(index));
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
