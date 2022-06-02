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
                .Include(c => c.Heuristics)
                .Include(c => c.Severities)
                .FirstOrDefault(c => c.Id == id);
        }

        public CodeSmellDefinition GetCodeSmellDefinitionByName(string name)
        {
            return _dbContext.CodeSmellDefinitions
                .Include(c => c.Heuristics)
                .Include(c => c.Severities)
                .FirstOrDefault(c => c.Name.Equals(name));
        }

        public IEnumerable<CodeSmellDefinition> GetAllCodeSmellDefinitions()
        {
            return _dbContext.CodeSmellDefinitions.ToList();
        }

        public void SaveCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition)
        {
            _dbContext.Update(codeSmellDefinition);
            _dbContext.SaveChanges();
        }

        public CodeSmellDefinition DeleteCodeSmellDefinition(int id)
        {
            var deletedCodeSmell = _dbContext.CodeSmellDefinitions.Remove(_dbContext.CodeSmellDefinitions.Find(id)).Entity;
            _dbContext.SaveChanges();
            return deletedCodeSmell;
        }

        public HeuristicDefinition GetHeuristic(int id)
        {
            return _dbContext.HeuristicDefinitions
                .FirstOrDefault(h => h.Id == id);
        }

        public IEnumerable<HeuristicDefinition> GetAllHeuristics()
        {
            return _dbContext.HeuristicDefinitions;
        }

        public void SaveHeuristic(HeuristicDefinition heuristic)
        {
            _dbContext.Update(heuristic);
            _dbContext.SaveChanges();
        }

        public HeuristicDefinition DeleteHeuristic(int id)
        {
            var deletedHeuristic = _dbContext.HeuristicDefinitions.Remove(_dbContext.HeuristicDefinitions.Find(id)).Entity;
            _dbContext.SaveChanges();
            return deletedHeuristic;
        }

        public SeverityDefinition GetSeverity(int id)
        {
            return _dbContext.SeverityDefinitions
                .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<SeverityDefinition> GetAllSeverities()
        {
            return _dbContext.SeverityDefinitions;
        }

        public void SaveSeverity(SeverityDefinition severity)
        {
            _dbContext.Update(severity);
            _dbContext.SaveChanges();
        }

        public SeverityDefinition DeleteSeverity(int id)
        {
            var deletedSeverity = _dbContext.SeverityDefinitions.Remove(_dbContext.SeverityDefinitions.Find(id)).Entity;
            _dbContext.SaveChanges();
            return deletedSeverity;
        }
    }
}
