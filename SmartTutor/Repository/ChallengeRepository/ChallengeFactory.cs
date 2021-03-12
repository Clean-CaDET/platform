using LibGit2Sharp;
using RepositoryCompiler.RepositoryAdapters;
using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository.ChallengeProjectRepository;
using System;
using System.Collections.Generic;

namespace SmartTutor.Repository.ChallengeRepository
{
    public class ChallengeFactory
    {
        private readonly ChallengeProjectFactory challengeProjectFactory;

        public ChallengeFactory()
        {
            challengeProjectFactory = new ChallengeProjectFactory();
        }

        public Dictionary<SmellType, List<Challenge>> CreateChallenges()
        {
            Dictionary<SmellType, List<Challenge>> challenges = new Dictionary<SmellType, List<Challenge>>();

            challenges.Add(SmellType.LONG_METHOD, CreateLongMethodChallenges());

            return challenges;
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
    }
}
