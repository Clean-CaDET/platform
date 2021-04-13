using SmartTutor.ContentModel.ProgressModel;
using SmartTutor.ContentModel.ProgressModel.Repository;
using SmartTutor.Exceptions;

namespace SmartTutor.ContentModel
{
    public class TraineeService : ITraineeService
    {
        private readonly ITraineeRepository _traineeRepository;

        public TraineeService(ITraineeRepository traineeRepository)
        {
            _traineeRepository = traineeRepository;
        }

        public Trainee RegisterTrainee(Trainee trainee)
        {
            var loadedTrainee = _traineeRepository.GetTraineeByIndex(trainee.StudentIndex);
            if (loadedTrainee != null)
            {
                trainee.Id = loadedTrainee.Id;
                if (trainee.AuralScore == 0) trainee.AuralScore = loadedTrainee.AuralScore;
                if (trainee.KinaestheticScore == 0) trainee.KinaestheticScore = loadedTrainee.KinaestheticScore;
                if (trainee.VisualScore == 0) trainee.VisualScore = loadedTrainee.VisualScore;
                if (trainee.ReadWriteScore == 0) trainee.ReadWriteScore = loadedTrainee.ReadWriteScore;
                _traineeRepository.UpdateTrainee(trainee);
                return trainee;
            }

            _traineeRepository.SaveTrainee(trainee);
            return trainee;
        }

        public Trainee LoginTrainee(string studentIndex)
        {
            var trainee = _traineeRepository.GetTraineeByIndex(studentIndex);
            if (trainee == null) throw new TraineeWIthStudentIndexNotFound(studentIndex);
            return trainee;
        }
    }
}