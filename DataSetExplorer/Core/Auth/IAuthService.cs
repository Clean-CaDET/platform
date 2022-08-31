using DataSetExplorer.Core.Annotations.Model;
using FluentResults;

namespace DataSetExplorer.Core.Auth
{
    public interface IAuthService
    {
        Result<Annotator> Save(Annotator annotatorDTO);
        Result<Annotator> GetAnnotatorByEmail(string email);
        Result<Annotator> GetAnnotatorById(int id);
    }
}