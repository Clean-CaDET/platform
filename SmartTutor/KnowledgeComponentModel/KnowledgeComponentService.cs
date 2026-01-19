using System.Collections.Generic;
using SmartTutor.KnowledgeComponentModel.KnowledgeComponents;
using SmartTutor.KnowledgeComponentModel.KnowledgeComponents.Repository;

namespace SmartTutor.KnowledgeComponentModel
{
    public class KnowledgeComponentService : IKnowledgeComponentService
    {
        private readonly IKnowledgeComponentRepository _knowledgeComponentRepository;

        public KnowledgeComponentService(IKnowledgeComponentRepository knowledgeComponentRepository)
        {
            _knowledgeComponentRepository = knowledgeComponentRepository;
        }

        public List<KnowledgeComponent> GetKnowledgeComponents()
        {
            return _knowledgeComponentRepository.GetKnowledgeComponents();
        }
    }
}