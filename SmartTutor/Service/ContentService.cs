using SmartTutor.ContentModel;
using SmartTutor.Repository;
using System;

namespace SmartTutor.Service
{
    // TODO: Integrate here: Trainee repository & Recommender system,
    public class ContentService
    {
        public IContentRepository ContentRepository;

        public ContentService(IContentRepository contentRepository)
        {
            ContentRepository = contentRepository;
        }

        internal EducationContent FindContentForIssue(SmellType issue, int indexOfContent)
        {
            return ContentRepository.FindEducationalContent(issue, indexOfContent);
        }
        
    }
}