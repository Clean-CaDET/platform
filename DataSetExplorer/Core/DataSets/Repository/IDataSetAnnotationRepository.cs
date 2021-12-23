using DataSetExplorer.Core.Annotations.Model;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IDataSetAnnotationRepository
    {
        Annotation GetDataSetAnnotation(int id);
        Annotator GetAnnotator(int id);
        CodeSmell GetCodeSmell(string name);
        void Update(Annotation annotation);
    }
}
