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

        public Learner Register(Learner learner)
        {
            var existingLearner = _learnerRepository.GetByIndex(learner.StudentIndex);
            if (existingLearner != null)
            {
                learner.Id = existingLearner.Id;
                if (learner.AuralScore == 0) learner.AuralScore = existingLearner.AuralScore;
                if (learner.KinaestheticScore == 0) learner.KinaestheticScore = existingLearner.KinaestheticScore;
                if (learner.VisualScore == 0) learner.VisualScore = existingLearner.VisualScore;
                if (learner.ReadWriteScore == 0) learner.ReadWriteScore = existingLearner.ReadWriteScore;
                _learnerRepository.Update(learner);
                return learner;
            }

            _learnerRepository.Save(learner);
            return learner;
        }

        public Learner Login(string studentIndex)
        {
            var learner = _learnerRepository.GetByIndex(studentIndex);
            if (learner == null) throw new LearnerWithStudentIndexNotFound(studentIndex);
            return learner;
        }
    }
}