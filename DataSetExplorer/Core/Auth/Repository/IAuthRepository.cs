using DataSetExplorer.Core.Annotations.Model;

namespace DataSetExplorer.Core.Auth.Repository
{
    public interface IAuthRepository
    {
        void RegisterAnnotator(Annotator annotator);
        Annotator GetAnnotatorByEmail(string email);
        Annotator GetAnnotatorById(int id);
    }
}
