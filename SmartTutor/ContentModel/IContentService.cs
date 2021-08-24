using SmartTutor.ContentModel.Lectures;
using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel
{
    public interface IContentService
    {
        List<Lecture> GetLectures();
        void CreateKnowledgeNode(KnowledgeNode node);
        void CreateLearningObjectSummary(LearningObjectSummary map);

        List<LearningObject> GetLearningObjectsByLearningObjectSummary(int losId);
        List<KnowledgeNode> GetKnowledgeNodesByLecture(int lectureId);
        List<LearningObjectSummary> GetLearningObjectSummariesByNode(int nodeId);
    }
}