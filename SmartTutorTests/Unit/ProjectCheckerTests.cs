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
        private readonly ProjectChecker _semanticProjectChecker;
        private readonly ProjectChecker _structuralProjectChecker;

        public ProjectCheckerTests()
        {
            Dictionary<string, List<ChallengeFulfillmentStrategy>> semanticStrategiesApplicableToSnippet = new();
            semanticStrategiesApplicableToSnippet.Add("Classes.Semantic.Pharmacist", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicNameChecker(new List<string> { "PharmacistInfo", "PharmacistId" }, new List<string> { "Pharmacist", "Id" }, new ChallengeHint(21))
            });
            semanticStrategiesApplicableToSnippet.Add("Classes.Semantic.Stocktake", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicNameChecker(new List<string> { "_nameStr" }, new List<string> { "_name", "Stocktake" }, new ChallengeHint(22))
            });
            _semanticProjectChecker = new ProjectChecker(semanticStrategiesApplicableToSnippet);

            Dictionary<string, List<ChallengeFulfillmentStrategy>> structuralStrategiesApplicableToSnippet = new();
            structuralStrategiesApplicableToSnippet.Add("Classes.Structural.PharmacyService", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicMetricChecker(
                    new List<MetricRangeRule> {
                        new MetricRangeRule(33701, "NMD", 1, 2, new ChallengeHint(337001)),
                        new MetricRangeRule(33702, "CLOC", 4, 25, new ChallengeHint(337002)),
                        new MetricRangeRule(33703, "LCOM", -1, 0, new ChallengeHint(337003)),
                        new MetricRangeRule(33704, "CBO", 0, 0, new ChallengeHint(337004))
                    },
                    new List<MetricRangeRule> {
                         new MetricRangeRule(33705, "MELOC", 1, 15, new ChallengeHint(337005))
                    }
                ),
                new BasicNameChecker(new List<string> (), new List<string> { "PharmacyService" }, new ChallengeHint(30))
            });
            structuralStrategiesApplicableToSnippet.Add("Classes.Structural.VacationSlot", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicMetricChecker(
                    new List<MetricRangeRule> {
                        new MetricRangeRule(33706, "NMD", 0, 1, new ChallengeHint(337006)),
                        new MetricRangeRule(33707, "LCOM", -1, 0, new ChallengeHint(337007)),
                        new MetricRangeRule(33708, "CBO", 0, 0, new ChallengeHint(337008))
                    },
                    new List<MetricRangeRule> {
                         new MetricRangeRule(33709, "MELOC", 1, 15, new ChallengeHint(337009))
                    }
                ),
                new BasicNameChecker(new List<string> (), new List<string> { "VacationSlot" }, new ChallengeHint(31))
            });
            structuralStrategiesApplicableToSnippet.Add("Classes.Structural.Stocktake", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicMetricChecker(
                    new List<MetricRangeRule> {
                        new MetricRangeRule(33710, "NMD", 0, 1, new ChallengeHint(337010)),
                        new MetricRangeRule(33711, "LCOM", -1, 0, new ChallengeHint(337011)),
                        new MetricRangeRule(33712, "CBO", 0, 0, new ChallengeHint(337012))
                    },
                    new List<MetricRangeRule> {
                         new MetricRangeRule(33713, "MELOC", 1, 15, new ChallengeHint(337013))
                    }
                ),
                new BasicNameChecker(new List<string> (), new List<string> { "Stocktake" }, new ChallengeHint(32))
            });
            structuralStrategiesApplicableToSnippet.Add("Classes.Structural.Weekend", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicMetricChecker(
                    new List<MetricRangeRule> {
                        new MetricRangeRule(33714, "NMD", 0, 1, new ChallengeHint(337014)),
                        new MetricRangeRule(33715, "LCOM", -1, 0, new ChallengeHint(337015)),
                        new MetricRangeRule(33716, "CBO", 0, 0, new ChallengeHint(337016))
                    },
                    new List<MetricRangeRule> {
                         new MetricRangeRule(33717, "MELOC", 1, 15, new ChallengeHint(337017))
                    }
                ),
                new BasicNameChecker(new List<string> (), new List<string> { "Weekend" }, new ChallengeHint(33))
            });
            structuralStrategiesApplicableToSnippet.Add("Classes.Structural.Pharmacist", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicMetricChecker(
                    new List<MetricRangeRule> {
                        new MetricRangeRule(33718, "NMD", 0, 1, new ChallengeHint(337018)),
                        new MetricRangeRule(33719, "LCOM", -1, 0, new ChallengeHint(337019)),
                        new MetricRangeRule(33720, "CBO", 1, 2, new ChallengeHint(337020))
                    },
                    new List<MetricRangeRule> {
                         new MetricRangeRule(33721, "MELOC", 1, 15, new ChallengeHint(337021))
                    }
                ),
                new BasicNameChecker(new List<string> (), new List<string> { "Pharmacist" }, new ChallengeHint(34))
            });
            structuralStrategiesApplicableToSnippet.Add("Classes.Structural.Run", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicMetricChecker(
                    new List<MetricRangeRule> {
                        new MetricRangeRule(33722, "NMD", 0, 1, new ChallengeHint(337022)),
                        new MetricRangeRule(33723, "LCOM", -1, 0, new ChallengeHint(337023)),
                        new MetricRangeRule(33724, "CBO", 0, 1, new ChallengeHint(337024))
                    },
                    new List<MetricRangeRule> {
                         new MetricRangeRule(33725, "MELOC", 1, 15, new ChallengeHint(337025))
                    }
                ),
                new BasicNameChecker(new List<string> (), new List<string> { "Run" }, new ChallengeHint(35))
            });
            _structuralProjectChecker = new ProjectChecker(structuralStrategiesApplicableToSnippet);
        }

        [Theory]
        [MemberData(nameof(SemanticProjectCheckerChallengeTest))]
        public void Evaluates_semantic_solution_submission(string[] submissionAttempt, List<ChallengeHint> expectedHints, bool expectedCompletion)
        {
            var challenge = new Challenge(51, 1, new List<ChallengeFulfillmentStrategy> { _semanticProjectChecker });

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

        [Theory]
        [MemberData(nameof(StructuralProjectCheckerChallengeTest))]
        public void Evaluates_structural_solution_submission(string[] submissionAttempt, List<ChallengeHint> expectedHints, bool expectedCompletion)
        {
            var challenge = new Challenge(52, 2, new List<ChallengeFulfillmentStrategy> { _structuralProjectChecker });

            var challengeEvaluation = challenge.CheckChallengeFulfillment(submissionAttempt, null);
            var actualHints = challengeEvaluation.ApplicableHints.GetHints();

            actualHints.Count.ShouldBe(expectedHints.Count);
            challengeEvaluation.ChallengeCompleted.ShouldBe(expectedCompletion);
        }

        public static IEnumerable<object[]> SemanticProjectCheckerChallengeTest =>
            new List<object[]>
            {
                new object[]
                {
                    SemanticProjectCheckerChallengeTestData.GetFourPassingClasses(),
                     new List<ChallengeHint>
                    {
                        new ChallengeHint(21),
                        new ChallengeHint(22)
                    },
                    true
                },
                new object[]
                {
                    SemanticProjectCheckerChallengeTestData.GetFourViolatingClasses(),
                    new List<ChallengeHint>(),
                    false
                }
            };

        public static IEnumerable<object[]> StructuralProjectCheckerChallengeTest =>
           new List<object[]>
           {
                new object[]
                {
                    StructuralProjectCheckerChallengeTestData.GetFourPassingClasses(),
                    new List<ChallengeHint>
                    {
                        new ChallengeHint(33701),
                        new ChallengeHint(33702),
                        new ChallengeHint(33703),
                        new ChallengeHint(33704),
                        new ChallengeHint(33705),
                        new ChallengeHint(33706),
                        new ChallengeHint(33707),
                        new ChallengeHint(33708),
                        new ChallengeHint(33709),
                        new ChallengeHint(33710),
                        new ChallengeHint(33711),
                        new ChallengeHint(33712),
                        new ChallengeHint(33713),
                        new ChallengeHint(33714),
                        new ChallengeHint(33715),
                        new ChallengeHint(33716),
                        new ChallengeHint(33717),
                        new ChallengeHint(33718),
                        new ChallengeHint(33719),
                        new ChallengeHint(33720),
                        new ChallengeHint(33721),
                        new ChallengeHint(33722),
                        new ChallengeHint(33723),
                        new ChallengeHint(33724),
                        new ChallengeHint(33725),
                        new ChallengeHint(30),
                        new ChallengeHint(31),
                        new ChallengeHint(32),
                        new ChallengeHint(33),
                        new ChallengeHint(34),
                        new ChallengeHint(35)
                    },
                    true
                },
                new object[]
                {
                    StructuralProjectCheckerChallengeTestData.GetFourViolatingClasses(),
                    new List<ChallengeHint>
                    {
                        new ChallengeHint(33701),
                        new ChallengeHint(33706)
                    },
                    false
                }
           };
    }
}