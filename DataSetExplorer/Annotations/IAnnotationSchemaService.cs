using System.Collections.Generic;
using DataSetExplorer.Annotations.Model;
using FluentResults;

namespace DataSetExplorer.Annotations
{
    public interface IAnnotationSchemaService
    {
        Result<CodeSmellDefinition> GetCodeSmellDefinition(int id);
        Result<CodeSmellDefinition> CreateCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition);
        Result<IEnumerable<CodeSmellDefinition>> GetAllCodeSmellDefinitions();
        Result<CodeSmellDefinition> UpdateCodeSmellDefinition(int id, CodeSmellDefinition codeSmellDefinition);
        Result<CodeSmellDefinition> DeleteCodeSmellDefinition(int id);
        Result<IEnumerable<Heuristic>> GetAllHeuristics(); 
        Result<Heuristic> CreateHeuristic(Heuristic heuristic);
        Result<Heuristic> UpdateHeuristic(int id, Heuristic heuristic);
        Result<Heuristic> DeleteHeuristic(int id);
        Result<IEnumerable<Heuristic>> AddHeuristicsToCodeSmell(int id, IEnumerable<Heuristic> heuristics);
        Result<IEnumerable<Heuristic>> GetHeuristicsForCodeSmell(int id);
        Result<CodeSmellHeuristic> DeleteHeuristicFromCodeSmell(int smellId, int heuristicId);
    }
}
