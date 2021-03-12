using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository;
using SmartTutor.Service;
using System.Collections.Generic;

namespace SmartTutor.Controllers
{
    public class ActiveEducationController
    {
        public ActiveEducationService ActiveEducationService;

        public ActiveEducationController()
        {
            ActiveEducationService = new ActiveEducationService(new ActivityRepository());
        }

        public List<EducationActivity> FindActivitiesForIssue(SmellType issue)
        {
            return ActiveEducationService.FindActivitiesForIssue(issue);
        }
    }
}
