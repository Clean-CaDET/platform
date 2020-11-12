using SmartTutor.ContentModel;
using System.Collections.Generic;

namespace SmartTutor.Repository
{
    public class ContentInMemoryRepository : IContentRepository
    {
        public Dictionary<SmellType, List<EducationalContent>> educationContents { get; set; }

        public ContentInMemoryRepository()
        {
            ContentInMemoryFactory contentInMemoryFactory = new ContentInMemoryFactory();
            educationContents = contentInMemoryFactory.createContent();
        }

        public List<EducationalContent> FindEducationalContent(SmellType issue)
        {
            return educationContents[issue];
        }
    }
}
