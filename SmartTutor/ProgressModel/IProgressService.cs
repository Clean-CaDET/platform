using SmartTutor.ProgressModel.Progress;
using System.Collections.Generic;

namespace SmartTutor.ProgressModel
{
    public interface IProgressService
    {
        List<NodeProgress> GetKnowledgeNodes(int lectureId, int? learnerId);
        NodeProgress GetNodeContent(int nodeId, int? learnerId);
    }
}