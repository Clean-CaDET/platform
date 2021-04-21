using System.IO;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.SourceCode
{
    public class SourceCodeChecker
    {
        public bool ValidSourceCode(string[] sourceCode)
        {
            CreateFileFromCode(sourceCode);
            return new ExecutableFileChecker().IsExecutableFile();
        }

        private void CreateFileFromCode(string[] sourceCode)
        {
            string path = @".\ContentModel\LearningObjects\ChallengeModel\SourceCode\FileStorage\SourceCode.cs";
            foreach (var line in sourceCode)
                File.WriteAllText(path, line);
        }
    }
}
