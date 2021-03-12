using RepositoryCompiler.CodeModel;
using SmartTutor.ActiveEducationModel;
using System.Collections.Generic;

namespace SmartTutor.Repository.ChallengeProjectRepository
{
    public class ChallengeProjectFactory
    {
        public Dictionary<SmellType, List<ChallengeProject>> CreateProjects()
        {
            Dictionary<SmellType, List<ChallengeProject>> challengeProjects = new Dictionary<SmellType, List<ChallengeProject>>();

            challengeProjects.Add(SmellType.LONG_METHOD, CreateLongMethodProjects());

            return challengeProjects;
        }

        private List<ChallengeProject> CreateLongMethodProjects()
        {
            List<ChallengeProject> longMethodProjects = new List<ChallengeProject>
            {
                CreateLongMethodProject()
            };

            return longMethodProjects;
        }

        public ChallengeProject CreateLongMethodProject()
        {
            return new ChallengeProject
            {
                Name = "Extract AwardAchievement method",
                Description = "1) Discover (e.g., using Google) the Extract Method command in your IDE." +
                                " 2) Using the command, extract multiple methods from the AwardAchievement method." +
                                " 3) For each method, define the most appropriate name.",
                Level = 1,
                Points = 5,
                GitURL = "https://github.com/Ana00000/Challenge-inspiration.git",
                StartState = new CodeModelFactory(LanguageEnum.CSharp).CreateProjectWithCodeFileLinks(@"C:\ActiveEducation\SRP\SRP1"),
                EndState = new CodeModelFactory(LanguageEnum.CSharp).CreateProjectWithCodeFileLinks(@"C:\ActiveEducation\SRP\SRP1")
            };
        }
    }
}
