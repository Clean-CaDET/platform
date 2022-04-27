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
                .FirstOrDefault(c => c.Id == id);
        }
        
        public void SaveCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition)
        {
            _dbContext.Update(codeSmellDefinition);
            _dbContext.SaveChanges();
        }

        public void SaveCodeSmellHeuristic(CodeSmellHeuristic codeSmellHeuristic)
        {
            _dbContext.Add(codeSmellHeuristic);
            _dbContext.SaveChanges();
        }

        public IEnumerable<HeuristicDefinition> GetHeuristicsForCodeSmell(int id)
        {
            return _dbContext.Heuristics.Where(h => h.CodeSmellHeuristics.Any(ch => ch.CodeSmellDefinitionId == id));
        }
        
        public CodeSmellHeuristic DeleteHeuristicFromCodeSmell(int smellId, int heuristicId)
        {
            var codeSmellHeuristic = _dbContext.CodeSmellHeuristics
                .First(ch => ch.CodeSmellDefinitionId == smellId && ch.HeuristicId == heuristicId);

            _dbContext.Remove(codeSmellHeuristic);
            _dbContext.SaveChanges();
            return codeSmellHeuristic;
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
            return _dbContext.Heuristics;
        }

        public HeuristicDefinition DeleteHeuristic(int id)
        {
            var deletedHeuristic = _dbContext.Heuristics.Remove(_dbContext.Heuristics.Find(id)).Entity;
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
            return _dbContext.Heuristics
                .FirstOrDefault(h => h.Id == id);
        }
    }
}
