using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.Auth.Repository;
using FluentResults;

namespace DataSetExplorer.Core.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public Result<Annotator> RegisterAnnotator(Annotator annotator)
        {
            _authRepository.RegisterAnnotator(annotator);
            return Result.Ok(annotator);
        }

        public Result<Annotator> GetAnnotatorByEmail(string email)
        {
            return Result.Ok(_authRepository.GetAnnotatorByEmail(email));
        }

        public Result<Annotator> GetAnnotatorById(int id)
        {
            return Result.Ok(_authRepository.GetAnnotatorById(id));
        }
    }
}