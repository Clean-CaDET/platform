using System.Collections.Generic;
using System.Linq;
using DataSetExplorer.Core.Annotations.Model;
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
            return _dbContext.Instances.Include(i => i.Annotations).FirstOrDefault(i => i.Id == id);
        }

        public Dictionary<string, List<Instance>> GetInstancesWithIdentifiersByDatasetId(int datasetId)
        {
            var instances = new Dictionary<string, List<Instance>>();
            var dataset = _dbContext.DataSets
                .Include(d => d.Projects)
                .ThenInclude(p => p.CandidateInstances)
                .ThenInclude(c => c.Instances)
                .ThenInclude(i => i.Identifiers)
                .FirstOrDefault(d => d.Id == datasetId);
            if (dataset == default) return null;

            foreach(var project in dataset.Projects)
                instances.Add(project.Name, project.CandidateInstances.SelectMany(c => c.Instances).ToList());
            
            return instances;
        }

        public Dictionary<string, List<Instance>> GetInstancesWithIdentifiersByProjectId(int projectId)
        {
            var instances = new Dictionary<string, List<Instance>>();
            var project = _dbContext.DataSetProjects
                .Include(p => p.CandidateInstances)
                .ThenInclude(c => c.Instances)
                .ThenInclude(i => i.Identifiers)
                .FirstOrDefault(p => p.Id == projectId);
            if (project == default) return null;

            instances.Add(project.Name, project.CandidateInstances.SelectMany(c => c.Instances).ToList());
            return instances;
        }

        public Dictionary<string, List<Instance>> GetInstancesByDatasetId(int datasetId)
        {
            var instances = new Dictionary<string, List<Instance>>();
            var dataset = _dbContext.DataSets
                .Include(d => d.Projects).ThenInclude(p => p.CandidateInstances).ThenInclude(c => c.Instances)
                .FirstOrDefault(d => d.Id == datasetId);
            if (dataset == default) return null;

            foreach (var project in dataset.Projects)
                instances.Add(project.Name, project.CandidateInstances.SelectMany(c => c.Instances).ToList());

            return instances;
        }

        public Dictionary<string, List<Instance>> GetInstancesByProjectId(int projectId)
        {
            var instances = new Dictionary<string, List<Instance>>();
            var project = _dbContext.DataSetProjects
                .Include(p => p.CandidateInstances).ThenInclude(c => c.Instances)
                .FirstOrDefault(p => p.Id == projectId);
            if (project == default) return null;

            instances.Add(project.Name, project.CandidateInstances.SelectMany(c => c.Instances).ToList());
            return instances;
        }

        public InstanceDTO GetInstanceWithRelatedInstances(int id)
        {
            return new InstanceDTO(_dbContext.Instances
                .Include(i => i.RelatedInstances)
                .FirstOrDefault(i => i.Id == id));
        }

        public Instance GetInstanceWithAnnotations(int id)
        {
            return _dbContext.Instances
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

        public List<SmellCandidateInstances> DeleteCandidateInstancesBySmell(List<CodeSmell> codeSmells)
        {
            var candidatesToDelete = new List<SmellCandidateInstances>();
            foreach (var codeSmell in codeSmells)
            {
                candidatesToDelete.AddRange(_dbContext.SmellCandidateInstances.Where(c => c.CodeSmell.Id == codeSmell.Id));
            }
            _dbContext.SmellCandidateInstances.RemoveRange(candidatesToDelete);
            _dbContext.SaveChanges();
            return candidatesToDelete;
        }
    }
}
