namespace SmartTutor.LearnerModel.Workspaces.Repository
{
    public interface IWorkspaceRepository
    {
        Workspace GetById(int learnerId);
        void Save(Workspace workspace);
    }
}
