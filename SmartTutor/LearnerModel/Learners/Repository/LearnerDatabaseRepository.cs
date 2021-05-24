using Microsoft.EntityFrameworkCore;
using SmartTutor.Database;
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
            return _dbContext.Learners.Where(l => l.Id == learnerId).Include(l => l.CourseEnrollments).FirstOrDefault();
        }
        public Learner GetByIndex(string index)
        {
            return _dbContext.Learners.Where(learner => learner.StudentIndex.Equals(index)).Include(l => l.CourseEnrollments).FirstOrDefault();
        }

        public Learner SaveOrUpdate(Learner learner)
        {
            _dbContext.Learners.Attach(learner);
            _dbContext.SaveChanges();
            return learner;
        }
    }
}
