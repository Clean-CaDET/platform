using DataSetExplorer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
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
                .Include(p => p.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator)
                .Include(p => p.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.ApplicableHeuristics)
                .FirstOrDefault(s => s.Id == id);
        }

        public void Update(DataSetProject dataSetProject)
        {
            _dbContext.Update(dataSetProject);
            _dbContext.SaveChanges();
        }
    }
}
