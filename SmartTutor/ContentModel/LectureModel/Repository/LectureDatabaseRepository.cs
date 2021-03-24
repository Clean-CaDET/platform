using System;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LectureModel.Repository
{
    public class LectureDatabaseRepository : ILectureRepository
    {
        private readonly LectureContext _dbContext;

        public LectureDatabaseRepository(LectureContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Lecture> GetLectures()
        {
            throw new NotImplementedException();
        }

        public List<KnowledgeNode> GetKnowledgeNodes(int id)
        {
            throw new NotImplementedException();
        }

        public KnowledgeNode GetKnowledgeNodeWithSummaries(int id)
        {
            throw new NotImplementedException();
        }
    }
}