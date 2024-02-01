using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.AnnotationSchema.Model;
using System.Collections.Generic;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IAnnotationRepository
    {
        Annotation Get(int id);
        Annotator GetAnnotator(int id);
        CodeSmell GetCodeSmell(string name);
        void Update(Annotation annotation);
        Annotation Delete(int id);
        SmellHeuristic DeleteHeuristic(int id);
        List<CodeSmell> GetCodeSmellsByDefinition(CodeSmellDefinition codeSmellDefinition);
        void DeleteCodeSmells(List<CodeSmell> codeSmells);
        void UpdateAppliedHeuristic(SmellHeuristic heuristic);
        CodeSmell UpdateCodeSmell(CodeSmell codeSmell);
    }
}
