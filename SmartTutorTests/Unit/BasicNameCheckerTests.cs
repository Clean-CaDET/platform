using RepositoryCompiler.Controllers;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutorTests.DataFactories;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.NameChecker;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class BasicNameCheckerTests
    {

        [Theory]
        [MemberData(nameof(ChallengeTest))]
        public void Evaluates_solution_submission(string[] submissionAttempt, List<ChallengeHint> expectedHints)
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

            var caDETClasses = new CodeRepositoryService().BuildClassesModel(submissionAttempt);
            var challengeEvaluation = challenge.CheckChallengeFulfillment(caDETClasses);
            var actualHints = challengeEvaluation.ApplicableHints.GetHints();

            actualHints.Count.ShouldBe(expectedHints.Count);
            actualHints.All(expectedHints.Contains).ShouldBeTrue();
        }

        public static IEnumerable<object[]> ChallengeTest =>
            new List<object[]>
            {
                new object[]
                {
                    //TODO: Find better names for these methods and add more test data and more thorough HintDirectory evaluation (probably separate tests for MetricRangeRules and HintDirectory)
                    ChallengeTestData.GetTwoPassingClasses(),
                    new List<ChallengeHint>()
                },
                new object[]
                {
                    ChallengeTestData.GetTwoViolatingClasses(),
                    new List<ChallengeHint>
                    {
                        new ChallengeHint {Id = 11},
                        new ChallengeHint {Id = 21}
                    }
                }
            };
    }
}
