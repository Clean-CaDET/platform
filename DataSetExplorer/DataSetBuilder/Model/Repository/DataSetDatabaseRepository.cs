using DataSetExplorer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public class DataSetDatabaseRepository : IDataSetRepository
    {
        private readonly DataSetExplorerContext _dbContext;

        public DataSetDatabaseRepository(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<DataSetExplorerContext>();
        }

        public void Create(DataSet dataSet)
        {
            _dbContext.Add(dataSet);
            _dbContext.SaveChanges();
        }

        public DataSet GetDataSet(int id)
        {
            return _dbContext.DataSets
                .Include(d => d.SupportedCodeSmells)
                .Include(s => s.Projects).ThenInclude(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(s => s.Projects).ThenInclude(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics)
                .Include(s => s.Projects).ThenInclude(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.InstanceSmell)
                .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<DataSet> GetAll()
        {
            return _dbContext.DataSets
                .Include(d => d.SupportedCodeSmells)
                .Include(s => s.Projects).ThenInclude(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(s => s.Projects).ThenInclude(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics)
                .Include(s => s.Projects).ThenInclude(p => p.CandidateInstances).ThenInclude(c => c.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.InstanceSmell);
        }

        public void Update(DataSet dataSet)
        {
            _dbContext.Update(dataSet);
            _dbContext.SaveChanges();
        }

        public Dictionary<string, List<string>> GetDataSetCodeSmells(int id)
        {
            var result = new Dictionary<string, List<string>>();
            var dataSet = _dbContext.DataSets.Include(d => d.SupportedCodeSmells).FirstOrDefault(s => s.Id == id);
            if (dataSet == null) return null;

            foreach (var smell in dataSet.SupportedCodeSmells)
            {
                result.Add(smell.Name, smell.RelevantSnippetTypes().Select(t => t.ToString()).ToList());
            }
            return result;
        }
    }
}
