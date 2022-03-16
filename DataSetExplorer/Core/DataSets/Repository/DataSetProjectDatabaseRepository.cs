using System.Collections.Generic;
using System.Linq;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataSetExplorer.Core.DataSets.Repository
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
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.CodeSmell)
                .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<DataSetProject> GetDataSetProjects(IEnumerable<int> projectIds)
        {
            return _dbContext.DataSetProjects
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.RelatedInstances)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.InstanceSmell)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.CodeSmell)
                .Where(p => projectIds.Contains(p.Id));
        }

        public DataSetProject Update(DataSetProject dataSetProject)
        {
            var updatedProject = _dbContext.Update(dataSetProject).Entity;
            _dbContext.SaveChanges();
            return updatedProject;
        }
        
        public DataSetProject Delete(int id)
        {
            var deletedProject = _dbContext.DataSetProjects.Remove(_dbContext.DataSetProjects.Find(id)).Entity;
            _dbContext.SaveChanges();
            return deletedProject;
        }
    }
}
