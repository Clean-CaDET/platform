using System.Linq;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public class GraphInstanceRepository : IGraphInstanceRepository
    {
        private readonly DataSetExplorerContext _dbContext;

        public GraphInstanceRepository(DataSetExplorerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GraphInstance GetGraphInstanceWithRelatedInstances(int projectId, string codeSnippetId)
        {
            var graphInstances = _dbContext.DataSetProjects
                .Include(p => p.GraphInstances).ThenInclude(i => i.RelatedInstances)
                .FirstOrDefault(p => p.Id == projectId).GraphInstances;

            return graphInstances.Find(i => i.CodeSnippetId.Equals(codeSnippetId));
        }
    }
}
