using System.Collections.Generic;

namespace SmartTutor.ContentModel.LectureModel.Repository
{
    public interface ILectureRepository
    {
        List<Lecture> GetLectures();
        List<KnowledgeNode> GetKnowledgeNodes(int id);
        KnowledgeNode GetKnowledgeNodeWithSummaries(int id);
    }
}