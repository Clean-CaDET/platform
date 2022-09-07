using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.Auth.Repository;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.Core.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public Result<Annotator> Save(Annotator annotator)
        {
            return Result.Ok(_authRepository.Save(annotator));
        }

        public Result<Annotator> GetAnnotatorByEmail(string email)
        {
            return Result.Ok(_authRepository.GetAnnotatorByEmail(email));
        }

        public Result<Annotator> GetAnnotatorById(int id)
        {
            return Result.Ok(_authRepository.GetAnnotatorById(id));
        }

        public Result<List<Annotator>> GetAll()
        {
            return Result.Ok(_authRepository.GetAll());
        }
    }
}