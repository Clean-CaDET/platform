using System.IO;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.SourceCode
{
    public class SourceCodeChecker
    {
        private readonly ExecutableFileChecker _executableFileChecker;

        public SourceCodeChecker()
        {
            _executableFileChecker = new ExecutableFileChecker();
        }

        public bool ValidSourceCode(string[] sourceCode)
        {
            CreateFileFromCode(sourceCode);
            return _executableFileChecker.IsExecutableFile();
        }

        private void CreateFileFromCode(string[] sourceCode)
        {
            string path = @"..\SourceCodeTests\FileStorage\SourceCodeFile.cs";
            foreach (var line in sourceCode)
                File.WriteAllText(path, line);
        }
    }
}
