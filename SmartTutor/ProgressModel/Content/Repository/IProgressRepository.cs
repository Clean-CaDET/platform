namespace SmartTutor.ProgressModel.Content.Repository
{
    public interface IProgressRepository
    {
        void SaveNodeProgress(NodeProgress nodeProgress);
        NodeProgress GetNodeProgressForLearner(int learnerId, int nodeId);
    }
}