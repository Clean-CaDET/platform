using SmartTutor.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.Repository
{
    public class ContentInMemoryRepository : IContentRepository
    {
        public Dictionary<SmellType, List<EducationContent>> educationContents { get; set; }

        public ContentInMemoryRepository()
        {
            ContentInMemoryFactory contentInMemoryFactory = new ContentInMemoryFactory();
            educationContents = contentInMemoryFactory.CreateContent();
        }

        public EducationContent FindEducationalContent(SmellType issue, int indexOfContent)
        {
            return educationContents[issue][indexOfContent];
        }

    }
}
