using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using System.Linq;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class ChallengeServiceTests
    {
        private readonly LearningObjectInMemoryRepository _learningObjectInMemoryRepository;

        public ChallengeServiceTests()
        {
            _learningObjectInMemoryRepository = new LearningObjectInMemoryRepository();
        }

        [Fact]
        public void Gets_proper_challenge_content()
        {
            ChallengeService challengeService = new ChallengeService(_learningObjectInMemoryRepository);

            Challenge challenge = challengeService.GetChallenge(3371);

            challenge.Id.ShouldBe(3371);
            challenge.LearningObjectSummaryId.ShouldBe(337);
            challenge.Url.ShouldBe("https://github.com/Ana00000/Challenge-inspiration.git");
            challenge.ResolvedClasses[0].Name.ShouldBe("Payment");
            challenge.ResolvedClasses[0].Members.Count().ShouldBe(2);
            challenge.ResolvedClasses[1].Name.ShouldBe("PaymentService");
            challenge.ResolvedClasses[1].Members.Count().ShouldBe(2);
            challenge.ResolvedClasses[1].Metrics.NMD.ShouldBe(2);
            challenge.ResolvedClasses[1].Members[0].Metrics.ELOC.ShouldBe(4);
            challenge.ResolvedClasses[1].Members[1].Metrics.ELOC.ShouldBe(3);
            challenge.ClassMetricRules[0].MetricName.ShouldBe("LOC");
            challenge.ClassMetricRules[1].ToValue.ShouldBe(2);
            challenge.MethodMetricRules[0].MetricName.ShouldBe("ELOC");
            challenge.MethodMetricRules[1].FromValue.ShouldBe(1);
        }
    }
}
