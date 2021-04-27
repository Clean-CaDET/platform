using SmartTutor.LearnerModel.Exceptions;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.LearnerModel.Learners.Repository;

namespace SmartTutor.LearnerModel
{
    public class LearnerService : ILearnerService
    {
        private readonly ILearnerRepository _learnerRepository;

        public LearnerService(ILearnerRepository learnerRepository)
        {
            _learnerRepository = learnerRepository;
        }

        public Learner Register(Learner newLearner)
        {
            var learner = _learnerRepository.GetByIndex(newLearner.StudentIndex);
            if (learner == null)
            {
                learner = newLearner;
            }
            else
            {
                learner.UpdateVARK(newLearner.VARKScore());
            }

            return _learnerRepository.SaveOrUpdate(learner);
        }

        public Learner Login(string studentIndex)
        {
            var learner = _learnerRepository.GetByIndex(studentIndex);
            if (learner == null) throw new LearnerWithStudentIndexNotFound(studentIndex);
            return learner;
        }
    }
}