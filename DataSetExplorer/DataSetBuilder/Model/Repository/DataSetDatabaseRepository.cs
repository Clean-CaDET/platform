using DataSetExplorer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
                .Include(s => s._projects).ThenInclude(p => p._instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(s => s._projects).ThenInclude(p => p._instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics)
                .FirstOrDefault(s => s.Id == id);
        }

        public void Update(DataSet dataSet)
        {
            _dbContext.Update(dataSet);
            _dbContext.SaveChanges();
        }
    }
}
