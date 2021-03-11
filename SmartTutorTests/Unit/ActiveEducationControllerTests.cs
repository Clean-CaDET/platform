using Shouldly;
using SmartTutor.Controllers;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class ActiveEducationControllerTests
    {
        [Fact]
        public void Calculates_number_of_activities_for_issue()
        {
            ActiveEducationController activeEducationController = new ActiveEducationController();

            var educationActivities = activeEducationController.FindActivitiesForIssue(SmellType.LONG_METHOD);

            educationActivities.Count.ShouldBe(2);
        }

    }
}
