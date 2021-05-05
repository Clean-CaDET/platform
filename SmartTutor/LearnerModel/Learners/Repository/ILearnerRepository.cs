namespace SmartTutor.LearnerModel.Learners.Repository
{
    public interface ILearnerRepository
    {
        Learner GetById(int learnerId);
        Learner GetByIndex(string index);
        Learner SaveOrUpdate(Learner learner);
    }
}
