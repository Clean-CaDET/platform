using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using Shouldly;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
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
            challenge.FulfillmentStrategy.ShouldNotBe(null);
        }
    }
}
