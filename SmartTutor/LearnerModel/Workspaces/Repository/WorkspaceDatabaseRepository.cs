using SmartTutor.Database;

namespace SmartTutor.LearnerModel.Workspaces.Repository
{
    public class WorkspaceDatabaseRepository : IWorkspaceRepository
    {
        private readonly SmartTutorContext _dbContext;

        public WorkspaceDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Workspace GetById(int learnerId)
        {
            return _dbContext.Workspaces.Find(learnerId);
        }

        public void Save(Workspace workspace)
        {
            _dbContext.Workspaces.Add(workspace);
            _dbContext.SaveChanges();
        }
    }
}
