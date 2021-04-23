using SmartTutor.ContentModel.Lectures;
using SmartTutor.ProgressModel.Content;
using System.Collections.Generic;

namespace SmartTutor.ContentModel
{
    public interface IContentService
    {
        List<Lecture> GetLectures();
        List<NodeProgress> GetKnowledgeNodes(int lectureId, int? traineeId);
        NodeProgress GetNodeContent(int nodeId, int? traineeId);
    }
}