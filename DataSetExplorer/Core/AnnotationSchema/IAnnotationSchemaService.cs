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
        Result<IEnumerable<HeuristicDefinition>> GetAllHeuristics(); 
        Result<HeuristicDefinition> CreateHeuristic(HeuristicDefinition heuristic);
        Result<HeuristicDefinition> UpdateHeuristic(int id, HeuristicDefinition heuristic);
        Result<HeuristicDefinition> DeleteHeuristic(int id);
        Result<IEnumerable<HeuristicDefinition>> AddHeuristicsToCodeSmell(int id, IEnumerable<HeuristicDefinition> heuristics);
        Result<IEnumerable<HeuristicDefinition>> GetHeuristicsForCodeSmell(int id);
        Result<CodeSmellHeuristic> DeleteHeuristicFromCodeSmell(int smellId, int heuristicId);
    }
}
