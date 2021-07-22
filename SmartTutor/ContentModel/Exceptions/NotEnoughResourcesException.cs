using System;

namespace SmartTutor.ContentModel.Exceptions
{
    public class NotEnoughResourcesException : Exception
    {
        public NotEnoughResourcesException() : base(
            $"You do not have enough resources left in your subscription to make this action!")
        {
        }
    }
}