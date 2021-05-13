namespace SmartTutor.LearnerModel.Workspaces
{
    public interface IWorkspaceCreator
    {
        public Workspace Create(int learnerId);
    }
}
