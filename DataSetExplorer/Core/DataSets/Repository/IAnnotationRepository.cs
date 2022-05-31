using DataSetExplorer.Core.Annotations.Model;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IAnnotationRepository
    {
        Annotation Get(int id);
        Annotator GetAnnotator(int id);
        CodeSmell GetCodeSmell(string name);
        void Update(Annotation annotation);
        SmellHeuristic DeleteHeuristic(int id);
    }
}
