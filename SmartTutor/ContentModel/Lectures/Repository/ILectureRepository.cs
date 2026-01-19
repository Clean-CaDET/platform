using System.Collections.Generic;

namespace SmartTutor.ContentModel.Lectures.Repository
{
    public interface ILectureRepository
    {
        Course GetCourse(int id);
        int GetCourseIdByLOId(int learningObjectSummaryId);
        Lecture GetLecture(int id);
        List<Lecture> GetLectures();
        List<KnowledgeNode> GetKnowledgeNodes(int id);
        KnowledgeNode GetKnowledgeNodeWithSummaries(int id);
        KnowledgeNode GetKnowledgeNodeBySummary(int id);
    }
}