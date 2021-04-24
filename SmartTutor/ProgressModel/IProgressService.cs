using System.Collections.Generic;
using SmartTutor.ProgressModel.Progress;

namespace SmartTutor.ProgressModel
{
    public interface IProgressService
    {
        List<NodeProgress> GetKnowledgeNodes(int lectureId, int? learnerId);
        NodeProgress GetNodeContent(int nodeId, int? learnerId);
    }
}