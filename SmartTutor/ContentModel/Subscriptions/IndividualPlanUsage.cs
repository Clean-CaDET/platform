namespace SmartTutor.ContentModel.Subscriptions
{
    public class IndividualPlanUsage
    {
        public int Id { get; private set; }
        public int IndividualPlanId { get; private set; }
        public int NumberOfUsersUsed { get; private set; }
        public int NumberOfCoursesUsed { get; private set; }
        public int NumberOfLecturesUsed { get; private set; }

        private IndividualPlanUsage(int id, int individualPlanId, int numberOfUsersUsed = 1,
            int numberOfCoursesUsed = 0,
            int numberOfLecturesUsed = 0)
        {
            Id = id;
            IndividualPlanId = individualPlanId;
            NumberOfUsersUsed = numberOfUsersUsed;
            NumberOfCoursesUsed = numberOfCoursesUsed;
            NumberOfLecturesUsed = numberOfLecturesUsed;
        }

        public IndividualPlanUsage(int individualPlanId) : this
            (0, individualPlanId, 1, 0, 0)
        {
        }

        public void IncrementNumberOfUsedLectures()
        {
            NumberOfLecturesUsed++;
        }

        public void IncrementNumberOfUsedCourses()
        {
            NumberOfCoursesUsed++;
        }
    }
}