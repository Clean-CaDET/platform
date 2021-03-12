using LibGit2Sharp;
using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository.ChallengeRepository;
using System;
using System.Collections.Generic;

namespace SmartTutor.Repository.ActiveEducationRepository
{
    public class ActivityFactory
    {
        private readonly ChallengeFactory challengeFactory;

        public ActivityFactory()
        {
            challengeFactory = new ChallengeFactory();
        }

        public Dictionary<SmellType, List<EducationActivity>> CreateActivities()
        {
            Dictionary<SmellType, List<EducationActivity>> educationActivities = new Dictionary<SmellType, List<EducationActivity>>();

            educationActivities.Add(SmellType.LONG_METHOD, CreateLongMethodActivities());

            return educationActivities;
        }

        private List<EducationActivity> CreateLongMethodActivities()
        {
            List<EducationActivity> longMethodActivities = new List<EducationActivity>();

            longMethodActivities.AddRange(challengeFactory.CreateLongMethodChallenges());
            longMethodActivities.AddRange(CreateLongMethodTrainings());

            return longMethodActivities;
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
