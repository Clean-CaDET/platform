using Shouldly;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.NameChecker;
using SmartTutor.Tests.TestData;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutor.Tests.Unit
{
    public class BasicNameCheckerTests
    {

        [Theory]
        [MemberData(nameof(ChallengeTest))]
        public void Evaluates_solution_submission(string[] submissionAttempt, List<ChallengeHint> expectedHints, bool expectedCompletion)
        {
            //TODO: Readonly lists
            var challenge = new Challenge(1, 1, new List<ChallengeFulfillmentStrategy>
            {
                new BasicNameChecker(null, new List<string> { "Payment", "PaymentService", "compensation" }, new ChallengeHint(11)),
                new BasicNameChecker(new List<string> { "Class", "List", "Method" }, null, new ChallengeHint(21))
            });

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
                    //TODO: Find better names for these methods and add more test data and more thorough HintDirectory evaluation (probably separate tests for MetricRangeRules and HintDirectory)
                    ChallengeTestData.GetTwoPassingClasses(),
                    new List<ChallengeHint>
                    {
                        new ChallengeHint(11),
                        new ChallengeHint(21)
                    },
                    true
                },
                new object[]
                {
                    ChallengeTestData.GetTwoViolatingClasses(),
                    new List<ChallengeHint>
                    {
                        new ChallengeHint(11),
                        new ChallengeHint(21)
                    },
                    false
                }
            };
    }
}
