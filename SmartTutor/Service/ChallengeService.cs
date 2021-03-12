using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository.ChallengeRepository;

namespace SmartTutor.Service
{
    public class ChallengeService
    {
        public IChallengeRepository ChallengeRepository;

        public ChallengeService(IChallengeRepository challengeRepository)
        {
            ChallengeRepository = challengeRepository;
        }

        public void StartChallenge(SmellType issue, int indexOfProject, Player player)
        {
            ChallengeRepository.StartChallenge(issue, indexOfProject, player);
        }

    }
}
