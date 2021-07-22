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

        public Annotator GetAnnotator(int id)
        {
            return _dbContext.Annotators.FirstOrDefault(a => a.Id == id);
        }

        public void AddAnnotation(DataSetInstance instance)
        {
            _dbContext.Update(instance);
            _dbContext.SaveChanges();
        }
    }
}
