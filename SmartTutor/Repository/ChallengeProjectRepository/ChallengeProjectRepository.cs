using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository.ActiveEducationRepository;
using System.Collections.Generic;

namespace SmartTutor.Repository.ChallengeProjectRepository
{
    public class ChallengeProjectRepository : IChallengeProjectRepository
    {
        public Dictionary<SmellType, List<ChallengeProject>> ChallengeProjects { get; set; }

        public ChallengeProjectRepository()
        {
            ChallengeProjects = new Dictionary<SmellType, List<ChallengeProject>>();

            List<ChallengeProject> challengeProjects = new List<ChallengeProject>();
            challengeProjects.Add(new ActivityFactory().CreateLongMethodProject());

            ChallengeProjects.Add(SmellType.LONG_METHOD, challengeProjects);
        }

        public ChallengeProject FindChallengeProjectForIssue(SmellType issue, int indexOfProject)
        {
            return ChallengeProjects[issue][indexOfProject];
        }
    }
}
