using System.Collections.Generic;
using System.Linq;
using RepositoryCompiler.Controllers;
using Shouldly;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.MetricChecker;
using SmartTutorTests.DataFactories;
using Xunit;

namespace SmartTutor.Tests.Unit
{
    public class BasicMetricsCheckerTests
    {
        private readonly BasicMetricChecker _basicMetricChecker;

        public BasicMetricsCheckerTests()
        {
            _basicMetricChecker = new BasicMetricChecker(
                new List<MetricRangeRule>
                {
                    new MetricRangeRule(33701,"CLOC",3,30,new ChallengeHint(337001)),
                    new MetricRangeRule (33702, "NMD", 0, 2, new ChallengeHint(5))
                },
                new List<MetricRangeRule>
                {
                    new MetricRangeRule(33703, "MELOC", 2, 5, new ChallengeHint(337002)),
                    new MetricRangeRule(33704, "NOP", 1, 4, new ChallengeHint(6))
                }
            );
        }

        [Theory]
        [MemberData(nameof(ChallengeTest))]
        public void Evaluates_solution_submission(string[] submissionAttempt, List<ChallengeHint> expectedHints)
        {
            var caDETClasses = new CodeRepositoryService().BuildClassesModel(submissionAttempt);
            var challengeEvaluation = _basicMetricChecker.EvaluateSubmission(caDETClasses);
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
                        new ChallengeHint(6),
                        new ChallengeHint(337002),
                        new ChallengeHint(337001)
                    }
                }
            };
    }
}
