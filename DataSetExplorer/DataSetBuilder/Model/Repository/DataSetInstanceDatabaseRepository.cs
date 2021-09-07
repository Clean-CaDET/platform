using DataSetExplorer.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var project = _dbContext.DataSetProjects.FirstOrDefault(p => p.Id == projectId);
            if (project == default) return new List<DataSetInstance>();
            return _dbContext.DataSetInstances.Where(i => i.ProjectLink.Equals(project.Url) && i.Annotations.Count > 0)
                .Where(i => annotatorId == null || i.Annotations.Any(a => a.Annotator.Id == annotatorId))
                .Include(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics)
                .Include(i => i.Annotations).ThenInclude(a => a.InstanceSmell);
        }

        public void Update(DataSetInstance instance)
        {
            _dbContext.Update(instance);
            _dbContext.SaveChanges();
        }
    }
}
