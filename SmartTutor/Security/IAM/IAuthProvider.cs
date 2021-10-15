using SmartTutor.LearnerModel.Learners;
using System.Threading.Tasks;

namespace SmartTutor.Security.IAM
{
    public interface IAuthProvider
    {
        Task<Learner> Register(Learner learner);
    }
}