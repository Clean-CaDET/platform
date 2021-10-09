using System.Collections.Generic;

namespace SmartTutor.KnowledgeComponentModel
{
    public class KnowledgeComponent
    {
        public int Id { get; private set; }
        
        public string Name { get; private set; }

        public List<KnowledgeComponent> KnowledgeSubcomponents { get; private set; }
        
    }
}