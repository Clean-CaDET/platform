using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel;

namespace SmartTutor.Repository
{
    public class ContentDatabaseRepository : IContentRepository
    {
        private readonly SmartTutorContext _dbContext;

        public ContentDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<EducationalContent> FindEducationalContent(SmellType smellType)
        {
            // TODO: move this logic to recommender
            return FindAll();
        }

        private List<EducationalContent> FindAll()
        {
            return _dbContext.EducationalContents.ToList();
        }
    }
}