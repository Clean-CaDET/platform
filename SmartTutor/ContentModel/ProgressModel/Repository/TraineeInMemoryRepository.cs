using System;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.ProgressModel.Repository
{
    public class TraineeInMemoryRepository : ITraineeRepository
    {
        public List<NodeProgress> GetActivatedNodes(int traineeId)
        {
            throw new NotImplementedException();
        }

        public Trainee GetTraineeById(int traineeId)
        {
            throw new NotImplementedException();
        }

        public void SaveNodeProgress(NodeProgress nodeProgress)
        {
            throw new NotImplementedException();
        }
    }
}
