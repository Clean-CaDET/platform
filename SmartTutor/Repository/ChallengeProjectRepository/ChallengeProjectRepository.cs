using SmartTutor.ActiveEducationModel;
using System.Collections.Generic;

namespace SmartTutor.Repository.ChallengeProjectRepository
{
    public class ChallengeProjectRepository : IChallengeProjectRepository
    {
        public Dictionary<SmellType, List<ChallengeProject>> ChallengeProjects { get; set; }

        public ChallengeProjectRepository()
        {
            ChallengeProjectFactory challengeProjectFactory = new ChallengeProjectFactory();
            ChallengeProjects = challengeProjectFactory.CreateProjects();
        }

        public ChallengeProject FindChallengeProjectForIssue(SmellType issue, int indexOfProject)
        {
            return ChallengeProjects[issue][indexOfProject];
        }
    }
}
