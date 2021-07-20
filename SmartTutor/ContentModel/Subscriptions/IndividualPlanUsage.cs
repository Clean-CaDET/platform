namespace SmartTutor.ContentModel.Subscriptions
{
    public class IndividualPlanUsage
    {
        public int Id { get; private set; }
        public IndividualPlan Plan { get; protected set; }

        public int NumberOfUsersUsed { get; protected set; }
        public int NumberOfCoursesUsed { get; private set; }
        public int NumberOfLecturesUsed { get; private set; }
        
        public int NumberOfUserLeft()
        {
            return Plan.NumberOfUsers - NumberOfUsersUsed;
        }

        public int NumberOfCoursesLeft()
        {
            return Plan.NumberOfCourses - NumberOfCoursesUsed;
        }

        public int NumberOfLecturesLeft()
        {
            return Plan.NumberOfLectures - NumberOfLecturesUsed;
        }
    }
}