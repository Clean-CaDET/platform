using System.Collections.Generic;
using DataSetExplorer.Core.AnnotationSchema.Model;
using FluentResults;

namespace DataSetExplorer.Core.AnnotationSchema
{
    public interface IAnnotationSchemaService
    {
        Result<CodeSmellDefinition> GetCodeSmellDefinition(int id);
        Result<CodeSmellDefinition> GetCodeSmellDefinitionByName(string name);
        Result<IEnumerable<CodeSmellDefinition>> GetAllCodeSmellDefinitions();
        Result<CodeSmellDefinition> CreateCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition);
        Result<CodeSmellDefinition> UpdateCodeSmellDefinition(int id, CodeSmellDefinition codeSmellDefinition);
        Result<CodeSmellDefinition> DeleteCodeSmellDefinition(int id);
        Result<IEnumerable<HeuristicDefinition>> GetHeuristicsForCodeSmell(int id);
        Result<IDictionary<string, HeuristicDefinition[]>> GetHeuristicsForEachCodeSmell();
        Result<HeuristicDefinition> AddHeuristicToCodeSmell(int id, HeuristicDefinition heuristic);
        Result<HeuristicDefinition> UpdateHeuristicInCodeSmell(int id, HeuristicDefinition heuristic);
        Result<CodeSmellDefinition> DeleteHeuristicFromCodeSmell(int smellId, int heuristicId);
        Result<IEnumerable<SeverityDefinition>> GetSeveritiesForCodeSmell(int id);
        Result<IDictionary<string, SeverityDefinition[]>> GetSeveritiesForEachCodeSmell();
        Result<SeverityDefinition> AddSeverityToCodeSmell(int id, SeverityDefinition severity);
        Result<SeverityDefinition> UpdateSeverityInCodeSmell(int id, SeverityDefinition severity);
        Result<CodeSmellDefinition> DeleteSeverityFromCodeSmell(int smellId, int severityId);
    }
}
