namespace SmartTutor.LearnerModel.Learners.Repository
{
    public interface ILearnerRepository
    {
        Learner GetById(int learnerId);
        Learner GetByIndex(string index);
        void Save(Learner learner);
        void Update(Learner learner);
    }
}
