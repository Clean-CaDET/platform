﻿using CodeModel;
using CodeModel.CaDETModel;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy;
using SmartTutor.ContentModel.Lectures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.Challenges
{
    public class Challenge : LearningObject
    {
        public string Url { get; private set; }
        public string Description { get; private set; }
        public LearningObjectSummary Solution { get; private set; }
        public List<ChallengeFulfillmentStrategy> FulfillmentStrategies { get; private set; }
        public string TestSuiteLocation { get; private set; }

        private Challenge() { }
        public Challenge(int id, int learningObjectSummaryId, string url, string description, LearningObjectSummary solution, List<ChallengeFulfillmentStrategy> fulfillmentStrategies) : base(id, learningObjectSummaryId)
        {
            Url = url;
            Description = description;
            Solution = solution;
            FulfillmentStrategies = fulfillmentStrategies;
        }

        public ChallengeEvaluation CheckChallengeFulfillment(string[] solutionAttempt)
        {
            CaDETProject solution = BuildCaDETModel(solutionAttempt);

            var evaluation = new ChallengeEvaluation(Id);
            foreach (var strategy in FulfillmentStrategies)
            {
                var result = strategy.EvaluateSubmission(solution.Classes);
                evaluation.ApplicableHints.MergeHints(result);
            }

            if (evaluation.ApplicableHints.IsEmpty())
            {
                evaluation.ChallengeCompleted = true;
                evaluation.ApplicableHints.AddAllHints(GetAllChallengeHints());
            }
            return evaluation;
        }

        private CaDETProject BuildCaDETModel(string[] sourceCode)
        {
            //TODO: Work with CaDETProject and consider introducing a list of compilation errors there.
            //TODO: Adhere to DIP for CodeModelFactory/CodeRepoService (extract interface and add DI in startup)
            var solutionAttempt = new CodeModelFactory().CreateProject(sourceCode);
            if (solutionAttempt.Classes == null || solutionAttempt.Classes.Count == 0) throw new InvalidOperationException("Invalid submission.");
            return solutionAttempt;
        }

        private List<ChallengeHint> GetAllChallengeHints()
        {
            return FulfillmentStrategies.SelectMany(s => s.GetAllHints().Where(h => h != null)).ToList();
        }
    }
}