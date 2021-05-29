using CodeModel;
using CodeModel.CaDETModel.CodeItems;
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
                new BasicNameChecker(new List<string> { "IsProfitableStocktakeForDay", "GetAllStocktakeResourcesNames" }, new List<string> { "Pharmacist", "HasAllVitaminsForDay", "GetAllNotProfitablePharmacistStocktakeMonthsForYear" }, new ChallengeHint(21))
            });
            semanticStrategiesApplicableToSnippet.Add("Classes.Semantic.Stocktake", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicNameChecker(new List<string> { "GetAllNotProfitablePharmacistStocktakeMonthsForYear" }, new List<string> { "Stocktake", "IsProfitableStocktakeForDay", "GetAllStocktakeResourcesNames" }, new ChallengeHint(22))
            });
            semanticStrategiesApplicableToSnippet.Add("Classes.Semantic.Run", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicNameChecker(new List<string> (), new List<string> { "Run" }, new ChallengeHint(23))
            });
            _semanticProjectChecker = new ProjectChecker(semanticStrategiesApplicableToSnippet);

            Dictionary<string, List<ChallengeFulfillmentStrategy>> structuralStrategiesApplicableToSnippet = new();
            structuralStrategiesApplicableToSnippet.Add("Classes.Structural.PharmacyService", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicMetricChecker(
                    new List<MetricRangeRule> {
                        new MetricRangeRule(33701, "NMD", 1, 4, new ChallengeHint(337001)),
                        new MetricRangeRule(33702, "CLOC", 25, 70, new ChallengeHint(337002)),
                        new MetricRangeRule(33703, "LCOM", -1, 1, new ChallengeHint(337003)),
                        new MetricRangeRule(33704, "CBO", -1, 2, new ChallengeHint(337004))
                    },
                    new List<MetricRangeRule> {
                         new MetricRangeRule(33705, "MELOC", -1, 15, new ChallengeHint(337005))
                    }
                ),
                new BasicNameChecker(new List<string> (), new List<string> { "PharmacyService" }, new ChallengeHint(30))
            });
            structuralStrategiesApplicableToSnippet.Add("Classes.Structural.Pharmacist", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicMetricChecker(
                    new List<MetricRangeRule> {
                        new MetricRangeRule(33706, "NMD", 0, 2, new ChallengeHint(337006)),
                        new MetricRangeRule(33707, "LCOM", -1, 0, new ChallengeHint(337007)),
                        new MetricRangeRule(33708, "CBO", -1, 0, new ChallengeHint(337008))
                    },
                    new List<MetricRangeRule> {
                         new MetricRangeRule(33709, "MELOC", -1, 15, new ChallengeHint(337009))
                    }
                ),
                new BasicNameChecker(new List<string> (), new List<string> { "Pharmacist" }, new ChallengeHint(31))
            });
            structuralStrategiesApplicableToSnippet.Add("Classes.Structural.Pill", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicMetricChecker(
                    new List<MetricRangeRule> {
                        new MetricRangeRule(33710, "NMD", 0, 1, new ChallengeHint(337010)),
                        new MetricRangeRule(33711, "LCOM", -1, 0, new ChallengeHint(337011)),
                        new MetricRangeRule(33712, "CBO", -1, 0, new ChallengeHint(337012))
                    },
                    new List<MetricRangeRule> {
                         new MetricRangeRule(33713, "MELOC", -1, 15, new ChallengeHint(337013))
                    }
                ),
                new BasicNameChecker(new List<string> (), new List<string> { "Pill" }, new ChallengeHint(32))
            });
            structuralStrategiesApplicableToSnippet.Add("Classes.Structural.Purchase", new List<ChallengeFulfillmentStrategy>()
            {
                new BasicMetricChecker(
                    new List<MetricRangeRule> {
                        new MetricRangeRule(33714, "NMD", 1, 3, new ChallengeHint(337014)),
                        new MetricRangeRule(33715, "LCOM", -1, 1, new ChallengeHint(337015)),
                        new MetricRangeRule(33716, "CBO", -1, 3, new ChallengeHint(337016))
                    },
                    new List<MetricRangeRule> {
                         new MetricRangeRule(33717, "MELOC", -1, 15, new ChallengeHint(337017))
                    }
                ),
                new BasicNameChecker(new List<string> (), new List<string> { "Purchase" }, new ChallengeHint(33))
            });
            _structuralProjectChecker = new ProjectChecker(structuralStrategiesApplicableToSnippet);
        }

        [Theory]
        [MemberData(nameof(SemanticProjectCheckerChallengeTest))]
        public void Evaluates_semantic_solution_submission(string[] submissionAttempt, List<ChallengeHint> expectedHints, bool expectedCompletion)
        {
            var challenge = new Challenge(101, 1, new List<ChallengeFulfillmentStrategy> { _semanticProjectChecker });

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
            List<CaDETClass> classes = new CodeModelFactory().CreateProject(submissionAttempt).Classes;
            var challenge = new Challenge(103, 2, new List<ChallengeFulfillmentStrategy> { _structuralProjectChecker });

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
                    SemanticProjectCheckerChallengeTestData.GetPassingClasses(),
                     new List<ChallengeHint>
                     {
                        new ChallengeHint(21),
                        new ChallengeHint(22),
                        new ChallengeHint(23)
                     },
                    true
                },
                new object[]
                {
                    SemanticProjectCheckerChallengeTestData.GetViolatingClasses(),
                    new List<ChallengeHint>
                    {
                        new ChallengeHint(21),
                        new ChallengeHint(22)
                    },
                    false
                }
            };

        public static IEnumerable<object[]> StructuralProjectCheckerChallengeTest =>
           new List<object[]>
           {
                new object[]
                {
                    StructuralProjectCheckerChallengeTestData.GetPassingClasses(),
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
                        new ChallengeHint(30),
                        new ChallengeHint(31),
                        new ChallengeHint(32),
                        new ChallengeHint(33)
                    },
                    true
                },
                new object[]
                {
                    StructuralProjectCheckerChallengeTestData.GetViolatingClasses(),
                    new List<ChallengeHint>
                    {
                        new ChallengeHint(337005),
                        new ChallengeHint(337014)
                    },
                    false
                }
           };
    }
}