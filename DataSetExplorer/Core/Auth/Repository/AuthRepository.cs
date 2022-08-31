using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataSetExplorer.Core.Auth.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataSetExplorerContext _dbContext;

        public AuthRepository(DataSetExplorerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Annotator Save(Annotator annotator)
        {
            _dbContext.Update(annotator);
            try
            {
                _dbContext.SaveChanges();
                return annotator;
            }
            catch (DbUpdateException e)
            {
                return null;
            }
        }

        public Annotator GetAnnotatorByEmail(string email)
        {
            return _dbContext.Annotators
                .FirstOrDefault(a => a.Email == email);
        }

        public Annotator GetAnnotatorById(int id)
        {
            return _dbContext.Annotators
                .FirstOrDefault(a => a.Id == id);
        }
    }
}
