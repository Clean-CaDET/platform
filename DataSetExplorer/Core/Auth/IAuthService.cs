using DataSetExplorer.Core.Annotations.Model;
using FluentResults;

namespace DataSetExplorer.Core.Auth
{
    public interface IAuthService
    {
        Result<Annotator> RegisterAnnotator(Annotator annotatorDTO);
        Result<Annotator> GetAnnotatorByEmail(string email);
        Result<Annotator> GetAnnotatorById(int id);
    }
}