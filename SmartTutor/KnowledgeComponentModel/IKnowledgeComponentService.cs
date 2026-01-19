using System.Collections.Generic;
using SmartTutor.KnowledgeComponentModel.KnowledgeComponents;

namespace SmartTutor.KnowledgeComponentModel
{
    public interface IKnowledgeComponentService
    {
        List<KnowledgeComponent> GetKnowledgeComponents();
    }
}