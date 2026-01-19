using Microsoft.Extensions.Options;
using System.IO;

namespace SmartTutor.LearnerModel.Workspaces
{
    public class WorkspaceFileCreator : IWorkspaceCreator
    {
        private readonly string _basePath;

        public WorkspaceFileCreator(IOptions<WorkspaceOptions> options)
        {
            _basePath = options.Value.BasePath;
        }
        public Workspace Create(int learnerId)
        {
            var relativeWorkspacePath = Path.Combine(learnerId.ToString(), "Workspace");
            var absoluteWorkspacePath = Path.Combine(_basePath, relativeWorkspacePath);
            if (!Directory.Exists(absoluteWorkspacePath))
            {
                CopyDirectory(Path.Combine(_basePath, "MasterWorkspace"), absoluteWorkspacePath);
            }

            return new Workspace(relativeWorkspacePath);
        }

        private static void CopyDirectory(string sourceDirName, string destDirName)
        {
            var sourceDirectory = new DirectoryInfo(sourceDirName);
            if (!sourceDirectory.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            Directory.CreateDirectory(destDirName);

            foreach (var subDirectory in sourceDirectory.GetDirectories())
            {
                if (subDirectory.Name.StartsWith(".")) continue;

                var destDirectory = Path.Combine(destDirName, subDirectory.Name);
                Directory.CreateDirectory(destDirectory);

                foreach (var file in subDirectory.GetFiles())
                {
                    var filePath = Path.Combine(destDirectory, file.Name);
                    File.Copy(file.FullName, filePath);
                }

                CopyDirectory(subDirectory.FullName, destDirectory);
            }
        }
    }
}
