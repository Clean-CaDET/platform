using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker;
using SmartTutor.ContentModel.LearningObjects.Repository;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class ChallengeServiceTests
    {
        private readonly ChallengeService _challengeService;

        public ChallengeServiceTests()
        {
            _challengeService = new ChallengeService(new LearningObjectInMemoryRepository());
        }

        [Fact]
        public void Gets_proper_challenge_content()
        {
            Challenge challenge = _challengeService.GetChallenge(3371);

            challenge.Id.ShouldBe(3371);
            challenge.LearningObjectSummaryId.ShouldBe(337);
            challenge.Url.ShouldBe("https://github.com/Ana00000/Challenge-inspiration.git");

            BasicMetricsChecker basicMetricsChecker = (BasicMetricsChecker)challenge.FulfillmentStrategy;
            basicMetricsChecker.ClassMetricRules.Count.ShouldBe(2);
            basicMetricsChecker.ClassMetricRules[0].Id.ShouldBe(33701);
            basicMetricsChecker.ClassMetricRules[0].MetricName.ShouldBe("CLOC");
            basicMetricsChecker.ClassMetricRules[0].FromValue.ShouldBe(3);
            basicMetricsChecker.ClassMetricRules[0].ToValue.ShouldBe(30);
            basicMetricsChecker.ClassMetricRules[1].Id.ShouldBe(33702);
            basicMetricsChecker.ClassMetricRules[1].MetricName.ShouldBe("NMD");
            basicMetricsChecker.ClassMetricRules[1].FromValue.ShouldBe(0);
            basicMetricsChecker.ClassMetricRules[1].ToValue.ShouldBe(2);
            basicMetricsChecker.MethodMetricRules.Count.ShouldBe(2);
            basicMetricsChecker.MethodMetricRules[0].Id.ShouldBe(33703);
            basicMetricsChecker.MethodMetricRules[0].MetricName.ShouldBe("MELOC");
            basicMetricsChecker.MethodMetricRules[0].FromValue.ShouldBe(2);
            basicMetricsChecker.MethodMetricRules[0].ToValue.ShouldBe(5);
            basicMetricsChecker.MethodMetricRules[1].Id.ShouldBe(33704);
            basicMetricsChecker.MethodMetricRules[1].MetricName.ShouldBe("NOP");
            basicMetricsChecker.MethodMetricRules[1].FromValue.ShouldBe(1);
            basicMetricsChecker.MethodMetricRules[1].ToValue.ShouldBe(4);

            challenge.FulfillmentStrategy.ChallengeHints.Count.ShouldBe(2);
            challenge.FulfillmentStrategy.ChallengeHints[0].Id.ShouldBe(337001);
            challenge.FulfillmentStrategy.ChallengeHints[0].Content.ShouldBe("Cohesion");
            challenge.FulfillmentStrategy.ChallengeHints[0].LearningObjectSummary.Id.ShouldBe(331);
            challenge.FulfillmentStrategy.ChallengeHints[0].LearningObjectSummary.Description.ShouldBe("Cohesion definition");
            challenge.FulfillmentStrategy.ChallengeHints[1].Id.ShouldBe(337002);
            challenge.FulfillmentStrategy.ChallengeHints[1].Content.ShouldBe("Cohesion");
            challenge.FulfillmentStrategy.ChallengeHints[1].LearningObjectSummary.Id.ShouldBe(336);
            challenge.FulfillmentStrategy.ChallengeHints[1].LearningObjectSummary.Description.ShouldBe("Structural cohesion example");
        }
    }
}
