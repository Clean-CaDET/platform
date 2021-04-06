using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Controllers;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker;
using SmartTutor.ContentModel.LectureModel;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using SmartTutorTests.DataFactories;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class BasicMetricsCheckerTests
    {
        private readonly BasicMetricsChecker _basicMetricsChecker;

        public BasicMetricsCheckerTests()
        {
            _basicMetricsChecker = new BasicMetricsChecker
            {
                ClassMetricRules = new List<MetricRangeRule>
                {
                    new MetricRangeRule
                    {
                        Id = 33701,
                        MetricName = "CLOC",
                        FromValue = 3,
                        ToValue = 30,
                        Hint = new ChallengeHint
                        {
                            Id = 337001,
                            Content = "Cohesion",
                            LearningObjectSummary = new LearningObjectSummary
                                {Id = 331, Description = "Cohesion definition"}
                        }
                    },
                    new MetricRangeRule {Id = 33702, MetricName = "NMD", FromValue = 0, ToValue = 2, Hint = new ChallengeHint {Id = 5}}
                },
                MethodMetricRules = new List<MetricRangeRule>
                {
                    new MetricRangeRule
                    {
                        Id = 33703,
                        MetricName = "MELOC",
                        FromValue = 2,
                        ToValue = 5,
                        Hint = new ChallengeHint
                        {
                            Id = 337002,
                            Content = "Cohesion",
                            LearningObjectSummary = new LearningObjectSummary
                                {Id = 336, Description = "Structural cohesion example"}
                        }
                    },
                    new MetricRangeRule {Id = 33704, MetricName = "NOP", FromValue = 1, ToValue = 4, Hint = new ChallengeHint {Id = 6}}
                }
            };
        }

        [Theory]
        [MemberData(nameof(ChallengeTest))]
        public void Evaluates_solution_submission(string[] submissionAttempt, List<ChallengeHint> expectedHints)
        {
            var caDETClasses = new CodeRepositoryService().BuildClassesModel(submissionAttempt);
            var challengeEvaluation = _basicMetricsChecker.EvaluateSubmission(caDETClasses);
            var actualHints = challengeEvaluation.GetHints().Values.SelectMany(h => h).ToList();

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
                        new ChallengeHint {Id = 6},
                        new ChallengeHint {Id = 337002}
                    }
                }
            };
    }
}
