using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository.ActiveEducationRepository;
using System.Collections.Generic;

namespace SmartTutor.Repository
{
    public class ActivityRepository : IActiveEducationRepository
    {
        public Dictionary<SmellType, List<EducationActivity>> EducationActivities { get; set; }

        public ActivityRepository()
        {
            ActivityFactory activityFactory = new ActivityFactory();
            EducationActivities = activityFactory.CreateActivities();
        }

        public List<EducationActivity> FindEducationActivitiesForIssue(SmellType issue)
        {
            return EducationActivities[issue];
        }

    }
}
