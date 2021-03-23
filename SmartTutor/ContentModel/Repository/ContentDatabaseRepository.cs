using System;
using System.Collections.Generic;
using SmartTutor.ContentModel.LectureModel;

namespace SmartTutor.ContentModel.Repository
{
    public class ContentDatabaseRepository : IContentRepository
    {
        private readonly ContentModelContext _dbContext;

        public ContentDatabaseRepository(ContentModelContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<LearningObject> FindEducationalContent(SmellType smellType)
        {
            throw new NotImplementedException();
        }
    }
}