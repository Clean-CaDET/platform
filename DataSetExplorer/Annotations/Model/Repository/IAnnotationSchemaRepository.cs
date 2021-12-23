using System.Collections.Generic;

namespace DataSetExplorer.Annotations.Model.Repository
{
    public interface IAnnotationSchemaRepository
    {
        CodeSmellDefinition GetCodeSmellDefinition(int id);
        void SaveCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition);
        IEnumerable<CodeSmellDefinition> GetAllCodeSmellDefinitions();
        CodeSmellDefinition DeleteCodeSmellDefinition(int id);
        IEnumerable<Heuristic> GetAllHeuristics();
        Heuristic DeleteHeuristic(int id);
        void SaveHeuristic(Heuristic heuristic);
        Heuristic GetHeuristic(int id);
        void SaveCodeSmellHeuristic(CodeSmellHeuristic codeSmellHeuristic);
        IEnumerable<Heuristic> GetHeuristicsForCodeSmell(int id);
        CodeSmellHeuristic DeleteHeuristicFromCodeSmell(int smellId, int heuristicId);
    }
}
