using Shouldly;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.Controllers;
using System.Collections.Generic;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class ChallengeControllerTests
    {
        private readonly ChallengeController _challengeController;

        public ChallengeControllerTests()
        {
            _challengeController = new ChallengeController();
        }

        [Fact]
        public void Gets_all_challenge_hints()
        {
            List<ChallengeHint> challengeHints = _challengeController.GetAllHints(3371);

            challengeHints.Count.ShouldBe(2);
            challengeHints[0].Id.ShouldBe(337001);
            challengeHints[0].Content.ShouldBe("Cohesion");
            challengeHints[0].LearningObjectSummary.Id.ShouldBe(331);
            challengeHints[0].LearningObjectSummary.Description.ShouldBe("Cohesion definition");
            challengeHints[1].Id.ShouldBe(337002);
            challengeHints[1].Content.ShouldBe("Cohesion");
            challengeHints[1].LearningObjectSummary.Id.ShouldBe(336);
            challengeHints[1].LearningObjectSummary.Description.ShouldBe("Structural cohesion example");
        }
    }
}
