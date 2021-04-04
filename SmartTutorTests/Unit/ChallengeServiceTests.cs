using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LearningObjects.Repository;
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
            challenge.FulfillmentStrategy.ChallengeHints.Count.ShouldBe(2);
            challenge.FulfillmentStrategy.ChallengeHints[0].Content.ShouldBe("Cohesion");
            challenge.FulfillmentStrategy.ChallengeHints[0].LearningObjectSummary.Id.ShouldBe(331);
            challenge.FulfillmentStrategy.ChallengeHints[0].LearningObjectSummary.Description.ShouldBe("Cohesion definition");
            challenge.FulfillmentStrategy.ChallengeHints[1].Content.ShouldBe("Cohesion");
            challenge.FulfillmentStrategy.ChallengeHints[1].LearningObjectSummary.Id.ShouldBe(336);
            challenge.FulfillmentStrategy.ChallengeHints[1].LearningObjectSummary.Description.ShouldBe("Structural cohesion example");
        }
    }
}
