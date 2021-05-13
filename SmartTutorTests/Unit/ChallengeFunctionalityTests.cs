using Shouldly;
using SmartTutor.ContentModel.LearningObjects.Challenges.FunctionalityTester;
using Xunit;

namespace SmartTutor.Tests.Unit
{
    public class ChallengeFunctionalityTests
    {
        [Fact]
        public void Returns_null_when_tests_pass()
        {
            IFunctionalityTester tester = new WorkspaceFunctionalityTester("../../../../");

            var evaluation = tester.IsFunctionallyCorrect(null, "SmellDetectorTests");

            evaluation.ShouldBeNull();
        }
    }
}
