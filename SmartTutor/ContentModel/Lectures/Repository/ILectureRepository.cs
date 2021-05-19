using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel.Lectures.Repository
{
    public interface ILectureRepository
    {
        Course GetCourse(int id);
        HashSet<int> GetCoursesIdsByLOsId(int learningObjectSummeryId);
        Lecture GetLecture(int id);
        List<Lecture> GetLectures();
        List<KnowledgeNode> GetKnowledgeNodes(int id);
        KnowledgeNode GetKnowledgeNodeWithSummaries(int id);
        List<KnowledgeNode> GetKnowledgeNodesBySummary(int id);
    }
}