using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartTutor.Database;

namespace SmartTutor.ContentModel.Lectures.Repository
{
    public class LectureDatabaseRepository : ILectureRepository
    {
        private readonly SmartTutorContext _dbContext;

        public LectureDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Course GetCourse(int id)
        {
            return _dbContext.Courses.FirstOrDefault(c => c.Id == id);
        }

        public int GetCourseIdByLOId(int learningObjectSummaryId)
        {
            var knowledgeNode = GetKnowledgeNodeBySummary(learningObjectSummaryId);

            var lecture = GetLecture(knowledgeNode.LectureId);

            return lecture.CourseId;
        }

        public Lecture GetLecture(int id)
        {
            return _dbContext.Lectures.FirstOrDefault(l => l.Id == id);
        }

        public List<Lecture> GetLectures()
        {
            return _dbContext.Lectures.Include(l => l.KnowledgeNodes).ToList();
        }

        public List<KnowledgeNode> GetKnowledgeNodes(int id)
        {
            var lecture = _dbContext.Lectures.Where(l => l.Id == id).Include(l => l.KnowledgeNodes).FirstOrDefault();
            return lecture?.KnowledgeNodes;
        }

        public KnowledgeNode GetKnowledgeNodeWithSummaries(int id)
        {
            return _dbContext.KnowledgeNodes.Where(n => n.Id == id).Include(n => n.LearningObjectSummaries)
                .FirstOrDefault();
        }

        public KnowledgeNode GetKnowledgeNodeBySummary(int id)
        {
            var learningObjectSummary = _dbContext.LearningObjectSummaries.Where(los => los.Id == id)
                .Include(los => los.KnowledgeNode).FirstOrDefault();
            return learningObjectSummary?.KnowledgeNode;
        }
    }
}