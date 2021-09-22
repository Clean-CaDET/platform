using DataSetExplorer.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public class DataSetInstanceDatabaseRepository : IDataSetInstanceRepository
    {
        private readonly DataSetExplorerContext _dbContext;

        public DataSetInstanceDatabaseRepository(DataSetExplorerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DataSetInstance GetDataSetInstance(int id)
        {
            return _dbContext.DataSetInstances.Include(i => i.Annotations).FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<DataSetInstance> GetInstancesAnnotatedByAnnotator(int projectId, int? annotatorId)
        {
            var project = _dbContext.DataSetProjects
                .Where(p => p.Id == projectId)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.CodeSmell)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.InstanceSmell)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator);
            if (project == default) return new List<DataSetInstance>();

            var instances = project.SelectMany(p => p.CandidateInstances)
                .SelectMany(c => c.Instances.Where(i => i.Annotations.Any(a => a.Annotator.Id == annotatorId)));
            return instances;
        }

        public IEnumerable<DataSetInstance> GetAnnotatedInstances(int projectId)
        {
            var project = _dbContext.DataSetProjects
                .Where(p => p.Id == projectId)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.CodeSmell)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.InstanceSmell);
            if (project == default) return new List<DataSetInstance>();

            var instances = project.SelectMany(p => p.CandidateInstances)
                .SelectMany(c => c.Instances.Where(i => i.Annotations.Count > 0));
            return instances;
        }

        public void Update(DataSetInstance instance)
        {
            _dbContext.Update(instance);
            _dbContext.SaveChanges();
        }
    }
}
