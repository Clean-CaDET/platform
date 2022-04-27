using System.Linq;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public class AnnotationDatabaseRepository : IAnnotationRepository
    {
        private readonly DataSetExplorerContext _dbContext;

        public AnnotationDatabaseRepository(DataSetExplorerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Annotation Get(int id)
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

        public CodeSmell GetCodeSmell(string name)
        {
            return _dbContext.CodeSmells.FirstOrDefault(s => s.Name.Equals(name));
        }

        public void Update(Annotation annotation)
        {
            _dbContext.Update(annotation);
            _dbContext.SaveChanges();
        }
    }
}
