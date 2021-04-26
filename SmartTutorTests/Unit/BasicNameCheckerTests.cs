using System.Collections.Generic;
using System.Linq;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.NameChecker;
using SmartTutorTests.DataFactories;
using Xunit;

namespace SmartTutor.Tests.Unit
{
    public class BasicNameCheckerTests
    {

        [Theory]
        [MemberData(nameof(ChallengeTest))]
        public void Evaluates_solution_submission(string[] submissionAttempt, List<ChallengeHint> expectedHints, bool expectedCompletion)
        {
            var challenge = new Challenge { FulfillmentStrategies = new List<ChallengeFulfillmentStrategy>
            {
                new BasicNameChecker
                {
                    Id = 1,
                    RequiredWords = new List<string> { "Payment", "PaymentService", "compensation" },
                    Hint = new ChallengeHint { Id = 11 }
                },

                new BasicNameChecker
                {
                    Id = 2,
                    BannedWords = new List<string> { "Class", "List", "Method" },
                    Hint = new ChallengeHint { Id = 21 }
                }
            }};

            var challengeEvaluation = challenge.CheckChallengeFulfillment(submissionAttempt);
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
                        new ChallengeHint {Id = 11},
                        new ChallengeHint {Id = 21}
                    },
                    true
                },
                new object[]
                {
                    ChallengeTestData.GetTwoViolatingClasses(),
                    new List<ChallengeHint>
                    {
                        new ChallengeHint {Id = 11},
                        new ChallengeHint {Id = 21}
                    },
                    false
                }
            };
    }
}
