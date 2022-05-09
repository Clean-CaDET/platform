using System.Collections.Generic;
using DataSetExplorer.Core.AnnotationSchema.Model;

namespace DataSetExplorer.Core.AnnotationSchema.Repository
{
    public interface IAnnotationSchemaRepository
    {
        CodeSmellDefinition GetCodeSmellDefinition(int id);
        void SaveCodeSmellDefinition(CodeSmellDefinition codeSmellDefinition);
        IEnumerable<CodeSmellDefinition> GetAllCodeSmellDefinitions();
        CodeSmellDefinition DeleteCodeSmellDefinition(int id);
        IEnumerable<HeuristicDefinition> GetAllHeuristics();
        HeuristicDefinition DeleteHeuristic(int id);
        void SaveHeuristic(HeuristicDefinition heuristic);
        HeuristicDefinition GetHeuristic(int id);
    }
}
