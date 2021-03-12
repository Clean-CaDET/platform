using SmartTutor.ActiveEducationModel;

namespace SmartTutor.Repository.ChallengeProjectRepository
{
    public interface IChallengeProjectRepository
    {
        ChallengeProject FindChallengeProjectForIssue(SmellType issue, int indexOfProject);
    }
}
