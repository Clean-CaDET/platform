using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.Subscriptions
{
    public class IndividualPlanUsage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public int IndividualPlanId { get; protected set; }
        public int NumberOfUsersUsed { get; protected set; }
        public int NumberOfCoursesUsed { get; private set; }
        public int NumberOfLecturesUsed { get; private set; }

        public IndividualPlanUsage(int id, int individualPlanId, int numberOfUsersUsed, int numberOfCoursesUsed, int numberOfLecturesUsed)
        {
            Id = id;
            IndividualPlanId = individualPlanId;
            NumberOfUsersUsed = numberOfUsersUsed;
            NumberOfCoursesUsed = numberOfCoursesUsed;
            NumberOfLecturesUsed = numberOfLecturesUsed;
        }
        
        public IndividualPlanUsage(int id, int individualPlanId)
        {
            Id = id;
            IndividualPlanId = individualPlanId;
            NumberOfUsersUsed = 1;
            NumberOfCoursesUsed = 0;
            NumberOfLecturesUsed = 0;
        }

        public IndividualPlanUsage(int individualPlanId)
        {
            IndividualPlanId = individualPlanId;
            NumberOfUsersUsed = 1;
            NumberOfCoursesUsed = 0;
            NumberOfLecturesUsed = 0;
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