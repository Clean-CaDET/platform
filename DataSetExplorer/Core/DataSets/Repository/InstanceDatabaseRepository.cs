using System.Collections.Generic;
using System.Linq;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Infrastructure.Database;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public class InstanceDatabaseRepository : IInstanceRepository
    {
        private readonly DataSetExplorerContext _dbContext;

        public InstanceDatabaseRepository(DataSetExplorerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Instance Get(int id)
        {
            return _dbContext.DataSetInstances.Include(i => i.Annotations).FirstOrDefault(i => i.Id == id);
        }

        public InstanceDTO GetInstanceWithRelatedInstances(int id)
        {
            var projects = _dbContext.DataSetProjects.Include(p => p.CandidateInstances)
                .ThenInclude(c => c.Instances).ToList();
            foreach (var project in projects)
            {
                foreach (var candidate in project.CandidateInstances.ToList())
                {
                    foreach (var instance in candidate.Instances.ToList())
                    {
                        if (instance.Id == id)
                        {
                            return new InstanceDTO(_dbContext.DataSetInstances
                                .Include(i => i.RelatedInstances)
                                .FirstOrDefault(i => i.Id == id), project.Id);
                        }
                    }
                }
            }
            return null;
        }

        public Instance GetInstanceWithAnnotations(int id)
        {
            return _dbContext.DataSetInstances
                .Include(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics)
                .FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Instance> GetInstancesAnnotatedByAnnotator(int projectId, int? annotatorId)
        {
            var project = _dbContext.DataSetProjects
                .Where(p => p.Id == projectId)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.CodeSmell)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.RelatedInstances)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.InstanceSmell)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator);
            if (project == default) return new List<Instance>();

            var instances = project.SelectMany(p => p.CandidateInstances)
                .SelectMany(c => c.Instances.Where(i => i.Annotations.Any(a => a.Annotator.Id == annotatorId)));
            return instances;
        }

        public IEnumerable<Instance> GetAnnotatedInstances(int projectId)
        {
            var project = _dbContext.DataSetProjects
                .Where(p => p.Id == projectId)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.CodeSmell)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.RelatedInstances)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.InstanceSmell)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics);

            if (project == default) return new List<Instance>();

            var instances = project.SelectMany(p => p.CandidateInstances)
                .SelectMany(c => c.Instances.Where(i => i.Annotations.Count > 0));
            return instances;
        }

        public void Update(Instance instance)
        {
            _dbContext.Update(instance);
            _dbContext.SaveChanges();
        }
    }
}
