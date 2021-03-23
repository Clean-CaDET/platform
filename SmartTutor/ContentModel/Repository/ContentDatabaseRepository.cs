using SmartTutor.ContentModel.LectureModel;
using System;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.Repository
{
    public class ContentDatabaseRepository : IContentRepository
    {
        private readonly ContentModelContext _dbContext;

        public ContentDatabaseRepository(ContentModelContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Lecture> GetLectures()
        {
            throw new NotImplementedException();
        }

        public Lecture GetLecture(int id)
        {
            throw new NotImplementedException();
        }
    }
}