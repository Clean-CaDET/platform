using Microsoft.Extensions.Options;
using SmartTutor.LearnerModel.Exceptions;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.LearnerModel.Learners.Workspaces;
using System;
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
            CreateWorkspaceForLearner(newLearner);
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

        private void CreateWorkspaceForLearner(Learner learner)
        {
            string learnerWorkspacePath = _workspaceOptions.BasePath + learner.Id + "/Workspace";
            if (Directory.Exists(learnerWorkspacePath)) return;

            Directory.CreateDirectory(learnerWorkspacePath);

            //_learnerRepository.Save(new Workspace(learner.Id, learnerWorkspacePath, DateTime.Now));

            AddFilesInLearnerWorkspace(learnerWorkspacePath);
        }

        private void AddFilesInLearnerWorkspace(string learnerWorkspacePath)
        {
            foreach (DirectoryInfo directoryInfo in new DirectoryInfo(_workspaceOptions.BasePath + "MasterTestSuite/").GetDirectories())
            {
                string directoryPath = learnerWorkspacePath + "/" + directoryInfo.Name;
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    string filePath = directoryPath + "/" + file.Name;
                    if (!File.Exists(filePath)) File.Copy(file.FullName, filePath);
                }
            }
        }
    }
}