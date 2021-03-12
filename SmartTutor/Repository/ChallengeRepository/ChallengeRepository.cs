using SmartTutor.ActiveEducationModel;
using SmartTutor.Repository.ActiveEducationRepository;
using System;
using System.Collections.Generic;

namespace SmartTutor.Repository.ChallengeRepository
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly ChallengeProjectRepository.ChallengeProjectRepository challengeProjectRepository;
        public Dictionary<SmellType, List<Challenge>> Challenges { get; set; }

        public ChallengeRepository()
        {
            Challenges = new Dictionary<SmellType, List<Challenge>>();
            Challenges.Add(SmellType.LONG_METHOD, new ActivityFactory().CreateLongMethodChallenges());
        }

        public void StartChallenge(SmellType issue, int indexOfProject, Player player)
        {
            Challenge challenge = new Challenge
            {
                Start = DateTime.Now,
                End = DateTime.Now,
                Status = ActivityStatus.Started,  //TODO: Prerequisites determine status
                Player = player,
                Project = challengeProjectRepository.FindChallengeProjectForIssue(issue, indexOfProject)
            };

            Challenges[issue].Add(challenge);
        }
    }
}
