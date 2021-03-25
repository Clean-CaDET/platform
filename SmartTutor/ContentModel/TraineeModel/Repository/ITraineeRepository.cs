using System.Collections.Generic;

namespace SmartTutor.ContentModel.TraineeModel.Repository
{
    public interface ITraineeRepository
    {
        /// <summary>
        /// Returns Nodes that have been completed or at least started.
        /// </summary>
        List<NodeProgress> GetActivatedNodes(int traineeId);
    }
}
