using System.Collections.Generic;
using System.Linq;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DataSetExplorer.Core.AnnotationSchema.Repository
{
    public class AnnotationSchemaDatabaseRepository : IAnnotationSchemaRepository
    {
        private readonly DataSetExplorerContext _dbContext;
        
        public AnnotationSchemaDatabaseRepository(DataSetExplorerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CodeSmellDefinition GetCodeSmellDefinition(int id)
        {
            return _dbContext.CodeSmellDefinitions
                .Include(c => c.SeverityRange)
                .Include(c => c.Heuristics)
                .FirstOrDefault(c => c.Id == id);
        }
        
        public void SaveCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition)
        {
            _dbContext.Update(codeSmellDefinition);
            _dbContext.SaveChanges();
        }

        public IEnumerable<CodeSmellDefinition> GetAllCodeSmellDefinitions()
        {
            return _dbContext.CodeSmellDefinitions
                .Include(c => c.SeverityRange);
        }

        public CodeSmellDefinition DeleteCodeSmellDefinition(int id)
        {
            var deletedCodeSmell = _dbContext.CodeSmellDefinitions.Remove(_dbContext.CodeSmellDefinitions.Find(id)).Entity;
            _dbContext.SaveChanges();
            return deletedCodeSmell;
        }

        public IEnumerable<HeuristicDefinition> GetAllHeuristics()
        {
            return _dbContext.HeuristicDefinitions;
        }

        public HeuristicDefinition DeleteHeuristic(int id)
        {
            var deletedHeuristic = _dbContext.HeuristicDefinitions.Remove(_dbContext.HeuristicDefinitions.Find(id)).Entity;
            _dbContext.SaveChanges();
            return deletedHeuristic;
        }

        public void SaveHeuristic(HeuristicDefinition heuristic)
        {
            _dbContext.Update(heuristic);
            _dbContext.SaveChanges();
        }

        public HeuristicDefinition GetHeuristic(int id)
        {
            return _dbContext.HeuristicDefinitions
                .FirstOrDefault(h => h.Id == id);
        }
    }
}
