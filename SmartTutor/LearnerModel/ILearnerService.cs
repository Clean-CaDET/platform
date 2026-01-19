using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.LearnerModel
{
    public interface ILearnerService
    {
        Learner Register(Learner learner);
        Learner Login(string studentIndex);
    }
}