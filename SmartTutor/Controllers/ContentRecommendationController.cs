using SmartTutor.ContentModel;
using SmartTutor.Service;
using System.Collections.Generic;

namespace SmartTutor.Controllers
{
    public class ContentRecommendationController
    {
        private readonly ContentService _contentService;

        public ContentRecommendationController(ContentService contentService)
        {
            _contentService = contentService;
        }

        public Dictionary<string, List<EducationalContent>> FindContentForIssue(Dictionary<string,List<SmellType>> issues, int indexOfContent)
        {
            return _contentService.FindContentForIssue(issues);
        }

    }
}
