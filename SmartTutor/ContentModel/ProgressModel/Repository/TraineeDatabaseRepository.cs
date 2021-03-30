using System.Collections.Generic;
using SmartTutor.ContentModel.LectureModel.Repository;
using SmartTutor.Database;

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
    }
}