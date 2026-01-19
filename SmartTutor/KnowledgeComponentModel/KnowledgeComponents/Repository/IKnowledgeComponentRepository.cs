using System.Collections.Generic;

namespace SmartTutor.KnowledgeComponentModel.KnowledgeComponents.Repository
{
    public interface IKnowledgeComponentRepository
    {
        List<KnowledgeComponent> GetKnowledgeComponents();
    }
}