using Shouldly;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.MetricChecker;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.NameChecker;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.ProjectChecker;
using SmartTutor.Tests.DataFactories;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutor.Tests.Unit
{
    public class ProjectCheckerTests
    {
        private readonly ProjectChecker _projectChecker;

        public ProjectCheckerTests()
        {
            Dictionary<string, List<ChallengeFulfillmentStrategy>> strategiesApplicableToSnippet = new();

            strategiesApplicableToSnippet.Add("Methods.Small.Payment", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicNameChecker(new List<string> { "Class", "List", "Method" }, null, new ChallengeHint(21)),
                new BasicMetricChecker(
                    new List<MetricRangeRule> { new MetricRangeRule(33701, "CLOC", 4, 6, new ChallengeHint(337001)) },
                    new List<MetricRangeRule>())
            });
            strategiesApplicableToSnippet.Add("Methods.Small.PaymentService.CreatePayment(int, int)", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicNameChecker(null, new List<string> { "CreatePayment" }, new ChallengeHint(11)),
                new BasicMetricChecker(
                    new List<MetricRangeRule>(),
                    new List<MetricRangeRule> { new MetricRangeRule(33704, "NOP", 1, 5, new ChallengeHint(6)) })
            });

            _projectChecker = new ProjectChecker(strategiesApplicableToSnippet);
        }

        [Theory]
        [MemberData(nameof(ChallengeTest))]
        public void Evaluates_solution_submission(string[] submissionAttempt, List<ChallengeHint> expectedHints, bool expectedCompletion)
        {
            var challenge = new Challenge(1, 1, new List<ChallengeFulfillmentStrategy> { _projectChecker });

            if (!expectedCompletion)
            {
                Should.Throw<KeyNotFoundException>(() => challenge.CheckChallengeFulfillment(submissionAttempt, null));
                return;
            }

            var challengeEvaluation = challenge.CheckChallengeFulfillment(submissionAttempt, null);
            var actualHints = challengeEvaluation.ApplicableHints.GetHints();

            actualHints.Count.ShouldBe(expectedHints.Count);
            actualHints.All(expectedHints.Contains).ShouldBeTrue();
            challengeEvaluation.ChallengeCompleted.ShouldBe(expectedCompletion);
        }

        public static IEnumerable<object[]> ChallengeTest =>
            new List<object[]>
            {
                new object[]
                {
                    ChallengeTestData.GetTwoPassingClasses(),
                     new List<ChallengeHint>
                    {
                        new ChallengeHint(6),
                        new ChallengeHint(11),
                        new ChallengeHint(21),
                        new ChallengeHint(337001)
                    },
                    true
                },
                new object[]
                {
                    ChallengeTestData.GetTwoViolatingClasses(),
                    new List<ChallengeHint>(),
                    false
                }
            };
    }
}