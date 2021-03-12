using SmartTutor.ActiveEducationModel;

namespace SmartTutor.Repository.ChallengeRepository
{
    public interface IChallengeRepository
    {
        void StartChallenge(SmellType issue, int indexOfProject, Player player);
    }
}
