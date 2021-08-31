using System.Threading.Tasks;
using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.SystemUser
{
    public interface IAuthProvider
    {
        Task<Learner> Register(Learner learner);
    }
}