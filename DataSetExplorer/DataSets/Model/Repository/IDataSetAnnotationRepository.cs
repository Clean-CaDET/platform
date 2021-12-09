namespace DataSetExplorer.DataSets.Model.Repository
{
    public interface IDataSetAnnotationRepository
    {
        Annotation GetDataSetAnnotation(int id);
        Annotator GetAnnotator(int id);
        CodeSmell GetCodeSmell(string name);
        void Update(Annotation annotation);
    }
}
