using SmartTutor.LearnerModel.Exceptions;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.LearnerModel.Workspaces;

namespace SmartTutor.LearnerModel
{
    public class LearnerService : ILearnerService
    {
        private readonly ILearnerRepository _learnerRepository;
        private readonly IWorkspaceCreator _workspaceCreator;

        public LearnerService(ILearnerRepository learnerRepository, IWorkspaceCreator workspaceCreator)
        {
            _learnerRepository = learnerRepository;
            _workspaceCreator = workspaceCreator;
        }

        public Learner Register(Learner newLearner)
        {
            var learner = SaveOrUpdate(newLearner);
            CreateWorkspace(learner);

            return learner;
        }

        private Learner SaveOrUpdate(Learner newLearner)
        {
            var learner = _learnerRepository.GetByIndex(newLearner.StudentIndex);
            if (learner == null)
            {
                learner = newLearner;
            }
            else
            {
                learner.UpdateVARK(newLearner.VARKScore());
            }

            return _learnerRepository.SaveOrUpdate(learner);
        }

        public Learner Login(string studentIndex)
        {
            var learner = _learnerRepository.GetByIndex(studentIndex);
            if (learner == null) throw new LearnerWithStudentIndexNotFound(studentIndex);
            return learner;
        }

        private void CreateWorkspace(Learner learner)
        {
            var workspace = _workspaceCreator.Create(learner.Id);
            if (workspace == null) return;

            learner.Workspace = workspace;
            _learnerRepository.SaveOrUpdate(learner);
        }
    }
}