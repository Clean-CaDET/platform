using System.Collections.Generic;
using DataSetExplorer.Core.AnnotationSchema.Model;
using FluentResults;

namespace DataSetExplorer.Core.AnnotationSchema
{
    public interface IAnnotationSchemaService
    {
        Result<CodeSmellDefinition> GetCodeSmellDefinition(int id);
        Result<CodeSmellDefinition> CreateCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition);
        Result<IEnumerable<CodeSmellDefinition>> GetAllCodeSmellDefinitions();
        Result<CodeSmellDefinition> UpdateCodeSmellDefinition(int id, CodeSmellDefinition codeSmellDefinition);
        Result<CodeSmellDefinition> DeleteCodeSmellDefinition(int id);
        Result<HeuristicDefinition> AddHeuristicToCodeSmell(int id, HeuristicDefinition heuristic);
        Result<IEnumerable<HeuristicDefinition>> GetHeuristicsForCodeSmell(int id);
        Result<CodeSmellDefinition> DeleteHeuristicFromCodeSmell(int smellId, int heuristicId);
        Result<HeuristicDefinition> UpdateHeuristicInCodeSmell(int id, HeuristicDefinition heuristic);
    }
}
