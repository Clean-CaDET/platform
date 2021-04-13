using System.Collections.Generic;

namespace SmartTutor.ContentModel.ProgressModel.Repository
{
    public interface ITraineeRepository
    {
        /// <summary>
        /// Returns Nodes that have been completed or at least started.
        /// </summary>
        List<NodeProgress> GetActivatedNodes(int traineeId);

        Trainee GetTraineeById(int traineeId);

        void SaveNodeProgress(NodeProgress nodeProgress);
        NodeProgress GetNodeProgressForTrainee(int traineeId, int nodeId);
        Trainee GetTraineeByIndex(string index);
        void SaveTrainee(Trainee trainee);
        void UpdateTrainee(Trainee trainee);
    }
}