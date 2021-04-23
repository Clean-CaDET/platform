using System.Collections.Generic;
using SmartTutor.ProgressModel.Progress;

namespace SmartTutor.ProgressModel
{
    public interface IProgressService
    {
        List<NodeProgress> GetKnowledgeNodes(int lectureId, int? traineeId);
        NodeProgress GetNodeContent(int nodeId, int? traineeId);
    }
}