using System;

namespace SmartTutor.ContentModel.Exceptions
{
    public class NumberOfDaysNotSupportedException : Exception
    {
        public NumberOfDaysNotSupportedException(int days) : base($"Number of days: {days} is not supported.")
        {
        }
    }
}