using System.Collections.Generic;

namespace SmartTutor.KnowledgeComponentModel.KnowledgeComponents
{
    public class KnowledgeComponent
    {
        public int Id { get; private set; }
        
        public string Name { get; private set; }

        public List<KnowledgeComponent> Subcomponents { get; private set; }
    }
}