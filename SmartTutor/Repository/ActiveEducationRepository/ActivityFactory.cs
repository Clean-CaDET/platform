using LibGit2Sharp;
using RepositoryCompiler.CodeModel;
using RepositoryCompiler.RepositoryAdapters;
using SmartTutor.ActiveEducationModel;
using System;
using System.Collections.Generic;

namespace SmartTutor.Repository.ActiveEducationRepository
{
    public class ActivityFactory
    {
        public Dictionary<SmellType, List<EducationActivity>> CreateActivities()
        {
            Dictionary<SmellType, List<EducationActivity>> educationActivities = new Dictionary<SmellType, List<EducationActivity>>();

            educationActivities.Add(SmellType.LONG_METHOD, CreateLongMethodActivities());

            return educationActivities;
        }

        private List<EducationActivity> CreateLongMethodActivities()
        {
            List<EducationActivity> longMethodActivities = new List<EducationActivity>();

            longMethodActivities.AddRange(CreateLongMethodChallenges());
            longMethodActivities.AddRange(CreateLongMethodTrainings());

            return longMethodActivities;
        }

        public List<Challenge> CreateLongMethodChallenges()
        {
            List<Challenge> longMethodChallenges = new List<Challenge>
            {
                CreateLongMethodChallenge()
            };

            return longMethodChallenges;
        }

        private Challenge CreateLongMethodChallenge()
        {
            new GitRepositoryAdapter().CloneRepository("https://github.com/Ana00000/Challenge-inspiration.git", "GGojko", "gojkoG9G8G7", "ActiveEducation");

            Challenge longMethodChallenge = new Challenge
            {
                Start = DateTime.Now,
                End = DateTime.Now,
                Status = ActivityStatus.Unlocked,
                Player = new Player
                {
                    Credentials = new UsernamePasswordCredentials { Username = "GGojko", Password = "gojkoG9G8G7" },
                    Title = new PlayerTitle { Name = "Novice", PointsWorth = 0 },
                    Score = 0,
                    Rank = 1,
                    EducationActivities = new List<EducationActivity>()
                },
                Project = CreateLongMethodProject()
            };

            return longMethodChallenge;
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

        private List<EducationTraining> CreateLongMethodTrainings()
        {
            List<EducationTraining> longMethodTrainings = new List<EducationTraining>
            {
                CreateLongMethodTraining()
            };

            return longMethodTrainings;
        }

        private EducationTraining CreateLongMethodTraining()
        {
            EducationTraining longMethodTraining = new EducationTraining
            {
                Start = DateTime.Now,
                End = DateTime.Now,
                Status = ActivityStatus.Unlocked,
                Player = new Player
                {
                    Credentials = new UsernamePasswordCredentials { Username = "MIRKOMM", Password = "963mmirkom369" },
                    Title = new PlayerTitle { Name = "Novice", PointsWorth = 0 },
                    Score = 0,
                    Rank = 2,
                    EducationActivities = new List<EducationActivity>()
                },
                Points = 5,
                Content = new ContentInMemoryFactory().CreateLongMethodContent()
            };

            return longMethodTraining;
        }
    }
}
