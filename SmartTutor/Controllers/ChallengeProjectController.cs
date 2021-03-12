using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository.ChallengeProjectRepository;
using SmartTutor.Service;

namespace SmartTutor.Controllers
{
    public class ChallengeProjectController
    {

        public ChallengeProjectService ChallengeProjectService;

        public ChallengeProjectController()
        {
            ChallengeProjectService = new ChallengeProjectService(new ChallengeProjectRepository());
        }

        public ChallengeProject FindProjectForIssue(SmellType issue, int indexOfProject)
        {
            return ChallengeProjectService.FindProjectForIssue(issue, indexOfProject);
        }
    }
}
