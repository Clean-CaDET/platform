using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

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
        List<KnowledgeNode> GetKnowledgeNodesByLecture(int lectureId);
        KnowledgeNode GetKnowledgeNode(int knowledgeNodeId);
        KnowledgeNode SaveOrUpdateKnowledgeNode(KnowledgeNode node);
        LearningObjectSummary SaveOrUpdateLearningObjectSummary(LearningObjectSummary learningObjectSummary);
        LearningObjectSummary GetLearningObjectSummary(int learningObjectSummaryId);
        List<LearningObject> GetLearningObjectsByLearningObjectSummary(int losId);
        List<LearningObjectSummary> GetLearningObjectSummariesByNode(int nodeId);
        void AddKnowledgeNodeToLecture(KnowledgeNode node, Lecture lecture);
        LearningObject SaveOrUpdateLearningObject(LearningObject learningObject);

        void AddLearningObjectSummaryToKnowledgeNode(LearningObjectSummary learningObjectSummary,
            KnowledgeNode knowledgeNode);
    }
}