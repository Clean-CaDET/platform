using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository;
using System.Collections.Generic;

namespace SmartTutor.Service
{
    public class ActiveEducationService
    {
        public IActiveEducationRepository ActiveEducationRepository;

        public ActiveEducationService(IActiveEducationRepository activeEducationRepository)
        {
            ActiveEducationRepository = activeEducationRepository;
        }

        public List<EducationActivity> FindActivitiesForIssue(SmellType issue)
        {
            return ActiveEducationRepository.FindEducationActivitiesForIssue(issue);
        }
    }
}
