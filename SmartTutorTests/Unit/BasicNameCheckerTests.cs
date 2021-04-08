using RepositoryCompiler.Controllers;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.NameChecker;
using SmartTutorTests.DataFactories;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class BasicNameCheckerTests
    {
        private readonly BasicNameChecker _basicNameChecker;

        public BasicNameCheckerTests()
        {
            _basicNameChecker = new BasicNameChecker
            {
                NamingRules = new List<NamingRule>
                {
                    new NamingRule
                    {
                        Id = 3370001,
                        BannedWords = new List<string> { "Class", "List", "Method" },
                        RequiredWords = new List<string> { "Payment", "Service", "PaymentService", "compensation" },
                        Hint = new ChallengeHint
                        {
                            Id = 337003,
                            Content = "Cohesion",
                            LearningObjectSummaryId = 336
                        },
                        MinLength = 3
                    },
                    new NamingRule
                    {
                        Id = 3370002,
                        BannedWords = new List<string> (),
                        RequiredWords = new List<string> { "Create", "Payment", "price" },
                        Hint = new ChallengeHint { Id = 7 }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(ChallengeTest))]
        public void Evaluates_solution_submission(string[] submissionAttempt, List<ChallengeHint> expectedHints)
        {
            var caDETClasses = new CodeRepositoryService().BuildClassesModel(submissionAttempt);
            var challengeEvaluation = _basicNameChecker.EvaluateSubmission(caDETClasses);
            var actualHints = challengeEvaluation.GetHints();

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
                        new ChallengeHint {Id = 7},
                        new ChallengeHint {Id = 337003}
                    }
                }
            };
    }
}
