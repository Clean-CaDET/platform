using SmartTutor.ContentModel;
using SmartTutor.Controllers;
using SmartTutor.Recommenders;
using SmartTutor.Repository;
using SmartTutor.Service;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartTutorTests.Integration
{
    public class ContentRecommendationControllerTests
    {
        [Fact]
        public void Gets_educational_content()
        {
            // GIVEN
            var issues = new Dictionary<string, List<SmellType>> {{"1", new List<SmellType> {SmellType.GOD_CLASS}}};
            var controller = new ContentRecommendationController(new ContentService(
                new KnowledgeBasedRecommender(new ContentDatabaseRepository(new SmartTutorContext()))));

            // WHEN
            var result = controller.FindContentForIssue(issues, 0);

            // THEN
            Assert.NotEmpty(result.Values.First());
        }
    }
}