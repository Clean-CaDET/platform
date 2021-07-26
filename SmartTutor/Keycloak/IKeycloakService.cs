using System.Threading.Tasks;
using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.Keycloak
{
    public interface IKeycloakService
    {
        Task<Learner> Register(Learner learner);
    }
}