using DataSetExplorer.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public class DataSetDatabaseRepository : IDataSetRepository
    {
        private readonly DataSetExplorerContext _dbContext;
        public DataSetDatabaseRepository(DataSetExplorerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(DataSet dataSet)
        {
            _dbContext.Add(dataSet);
            _dbContext.SaveChanges();
        }

        public DataSet GetDataSet(int id)
        {
            return _dbContext.DataSets.Include(s => s.Instances).ThenInclude(i => i.Annotations).ThenInclude(a => a.Annotator).FirstOrDefault(s => s.Id == id);
        }
    }
}
