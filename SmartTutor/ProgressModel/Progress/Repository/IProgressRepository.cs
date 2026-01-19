namespace SmartTutor.ProgressModel.Progress.Repository
{
    public interface IProgressRepository
    {
        void SaveNodeProgress(NodeProgress nodeProgress);
        NodeProgress GetNodeProgressForLearner(int learnerId, int nodeId);
    }
}