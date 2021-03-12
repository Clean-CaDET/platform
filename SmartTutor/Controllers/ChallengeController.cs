using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository.ChallengeRepository;
using SmartTutor.Service;

namespace SmartTutor.Controllers
{
    public class ChallengeController
    {
        public ChallengeService ChallengeService;

        public ChallengeController()
        {
            ChallengeService = new ChallengeService(new ChallengeRepository());
        }

        public void StartChallenge(SmellType issue, int indexOfProject, Player player)
        {
            ChallengeService.StartChallenge(issue, indexOfProject, player);
        }

    }
}
