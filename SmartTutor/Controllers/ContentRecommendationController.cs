using SmartTutor.ContentModel;
using SmartTutor.Repository;
using SmartTutor.Service;
using SmartTutor.Recommenders;
using System.Collections.Generic;

namespace SmartTutor.Controllers
{
    public class ContentRecommendationController
    {
        public ContentService ContentService;

        public ContentRecommendationController()
        {
            ContentService = new ContentService(new KnowledgeBasedRecommender(new ContentInMemoryRepository()));
        }

        public Dictionary<string, List<EducationalContent>> FindContentForIssue(Dictionary<string,List<SmellType>> issues, int indexOfContent)
        {
            return ContentService.FindContentForIssue(issues);
        }

    }
}
