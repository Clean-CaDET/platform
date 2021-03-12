using LibGit2Sharp;
using RepositoryCompiler.RepositoryAdapters;
using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository.ChallengeProjectRepository;
using System;
using System.Collections.Generic;

namespace SmartTutor.Repository.ActiveEducationRepository
{
    public class ActivityFactory
    {
        private readonly ChallengeProjectFactory challengeProjectFactory;

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
                Project = challengeProjectFactory.CreateLongMethodProject()
            };

            return longMethodChallenge;
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
