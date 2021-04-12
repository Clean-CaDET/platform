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
            if (_traineeRepository.GetTraineeByIndex(trainee.StudentIndex) != null)
            {
                throw new TraineeWithStudentIndexAlreadyExists();
            }

            _traineeRepository.SaveTrainee(trainee);
            return trainee;
        }

        public Trainee LoginTrainee(string studentIndex)
        {
            var trainee = _traineeRepository.GetTraineeByIndex(studentIndex);
            if (trainee == null) throw new TraineeWIthStudentIndexNotFound();
            return trainee;
        }
    }
}