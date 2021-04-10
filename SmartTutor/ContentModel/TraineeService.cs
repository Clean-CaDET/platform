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

        public void RegisterTrainee(Trainee trainee)
        {
            if (_traineeRepository.GetTraineeByIndex(trainee.StudentIndex) != null)
            {
                throw new TraineeWithStudentIndexAlreadyExists();
            }

            _traineeRepository.SaveTrainee(trainee);
        }
    }
}