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
    }
}
