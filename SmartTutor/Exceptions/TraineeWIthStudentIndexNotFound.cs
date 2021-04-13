using System;

namespace SmartTutor.Exceptions
{
    public class TraineeWIthStudentIndexNotFound : Exception
    {
        public TraineeWIthStudentIndexNotFound(string index) : base(
            $"Trainee with student index: {index} does not exist.")
        {
        }
    }
}