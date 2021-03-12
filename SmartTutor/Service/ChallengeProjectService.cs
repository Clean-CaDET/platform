using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository.ChallengeProjectRepository;

namespace SmartTutor.Service
{
    public class ChallengeProjectService
    {
        public IChallengeProjectRepository ChallengeProjectRepository;

        public ChallengeProjectService(IChallengeProjectRepository challengeProjectRepository)
        {
            ChallengeProjectRepository = challengeProjectRepository;
        }

        public ChallengeProject FindProjectForIssue(SmellType issue, int indexOfProject)
        {
            return ChallengeProjectRepository.FindChallengeProjectForIssue(issue, indexOfProject);
        }
    }
}
