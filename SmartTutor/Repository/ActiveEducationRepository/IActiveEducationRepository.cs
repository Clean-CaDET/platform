using SmartTutor.ActiveEducationModel;
using System.Collections.Generic;

namespace SmartTutor.Repository
{
    public interface IActiveEducationRepository
    {
        List<EducationActivity> FindEducationActivitiesForIssue(SmellType issue);
    }
}
