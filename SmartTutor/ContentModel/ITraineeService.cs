using SmartTutor.ContentModel.ProgressModel;

namespace SmartTutor.ContentModel
{
    public interface ITraineeService
    {
        Trainee RegisterTrainee(Trainee trainee);
        Trainee LoginTrainee(string studentIndex);
    }
}