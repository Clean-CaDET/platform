using Microsoft.Extensions.Options;
using SmartTutor.LearnerModel.Exceptions;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.LearnerModel.Options;
using System.IO;

namespace SmartTutor.LearnerModel
{
    public class LearnerService : ILearnerService
    {
        private readonly ILearnerRepository _learnerRepository;
        private readonly WorkspaceOptions _workspaceOptions;

        public LearnerService(ILearnerRepository learnerRepository, IOptions<WorkspaceOptions> workspaceOptions)
        {
            _learnerRepository = learnerRepository;
            _workspaceOptions = workspaceOptions.Value;
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

        //TODO: Examine if the Learner model should be a focused module, similar to the Instructor, that only has the learning behavior aspects (i.e., this logic is related to progress in that case)
        private void CreateWorkspace(Learner learner)
        {
            var learnerWorkspacePath = Path.Combine(_workspaceOptions.BasePath, learner.Id.ToString(), "Workspace");
            if (Directory.Exists(learnerWorkspacePath)) return;

            DirectoryCopy(Path.Combine(_workspaceOptions.BasePath, "MasterWorkspace"), learnerWorkspacePath);

            learner.SetWorkspace(learnerWorkspacePath);
            _learnerRepository.SaveOrUpdate(learner);
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            var sourceDirectory = new DirectoryInfo(sourceDirName);
            if (!sourceDirectory.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            Directory.CreateDirectory(destDirName);

            foreach (var subDirectory in sourceDirectory.GetDirectories())
            {
                if(subDirectory.Name.StartsWith(".")) continue;

                var destDirectory = Path.Combine(destDirName, subDirectory.Name);
                Directory.CreateDirectory(destDirectory);

                foreach (var file in subDirectory.GetFiles())
                {
                    var filePath = Path.Combine(destDirectory, file.Name);
                    File.Copy(file.FullName, filePath);
                }

                DirectoryCopy(subDirectory.FullName, destDirectory);
            }
        }
    }
}