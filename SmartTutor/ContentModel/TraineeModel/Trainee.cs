using System.Collections.Generic;

namespace SmartTutor.ContentModel.TraineeModel
{
    public class Trainee
    {
        public int Id { get; set; }
        public List<NodeProgress> Progress { get; set; }
    }
}