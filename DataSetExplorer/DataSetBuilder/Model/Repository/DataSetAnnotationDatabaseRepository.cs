using DataSetExplorer.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public class DataSetAnnotationDatabaseRepository : IDataSetAnnotationRepository
    {

        private readonly DataSetExplorerContext _dbContext;

        public DataSetAnnotationDatabaseRepository(DataSetExplorerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DataSetAnnotation GetDataSetAnnotation(int id)
        {
            return _dbContext.DataSetAnnotations
                .Include(a => a.Annotator)
                .Include(a => a.ApplicableHeuristics)
                .FirstOrDefault(a => a.Id == id);
        }
        public Annotator GetAnnotator(int id)
        {
            return _dbContext.Annotators.FirstOrDefault(a => a.Id == id);
        }

        public void Update(DataSetAnnotation annotation)
        {
            _dbContext.Update(annotation);
            _dbContext.SaveChanges();
        }
    }
}
