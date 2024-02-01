using DataSetExplorer.Core.Annotations.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.Core.Auth
{
    public interface IAuthService
    {
        Result<Annotator> Save(Annotator annotatorDTO);
        Result<Annotator> GetAnnotatorByEmail(string email);
        Result<Annotator> GetAnnotatorById(int id);
        Result<List<Annotator>> GetAll();
    }
}