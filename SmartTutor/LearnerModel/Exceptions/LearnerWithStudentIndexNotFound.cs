using System;

namespace SmartTutor.LearnerModel.Exceptions
{
    public class LearnerWithStudentIndexNotFound : Exception
    {
        public LearnerWithStudentIndexNotFound(string index) : base(
            $"Learner with student index: {index} does not exist.")
        {
        }
    }
}