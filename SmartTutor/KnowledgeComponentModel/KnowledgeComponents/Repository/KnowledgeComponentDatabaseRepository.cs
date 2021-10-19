using System.Collections.Generic;
using System.Linq;
using SmartTutor.Database;

namespace SmartTutor.KnowledgeComponentModel.KnowledgeComponents.Repository
{
    public class KnowledgeComponentDatabaseRepository : IKnowledgeComponentRepository
    {
        private readonly SmartTutorContext _dbContext;

        public KnowledgeComponentDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<KnowledgeComponent> GetKnowledgeComponents()
        {
            return _dbContext.KnowledgeComponents.ToList();
        }
    }
}