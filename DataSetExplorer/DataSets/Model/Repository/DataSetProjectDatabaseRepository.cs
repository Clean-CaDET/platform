using DataSetExplorer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSets.Model.Repository
{
    public class DataSetProjectDatabaseRepository : IDataSetProjectRepository
    {
        private readonly DataSetExplorerContext _dbContext;

        public DataSetProjectDatabaseRepository(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<DataSetExplorerContext>();
        }

        public DataSetProject GetDataSetProject(int id)
        {
            return _dbContext.DataSetProjects
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.InstanceSmell)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.CodeSmell)
                .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<DataSetProject> GetDataSetProjects(IEnumerable<int> projectIds)
        {
            return _dbContext.DataSetProjects
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.InstanceSmell)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.CodeSmell)
                .Where(p => projectIds.Contains(p.Id));
        }

        public void Update(DataSetProject dataSetProject)
        {
            _dbContext.Update(dataSetProject);
            _dbContext.SaveChanges();
        }
    }
}
