using System;
using System.IO;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.SourceCode
{
    public class ExecutableFileChecker
    {
        private byte[] FileContent { get; set; }

        public ExecutableFileChecker()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "\\FileStorage\\SourceCode.cs");
            FileContent = File.ReadAllBytes(filePath);
        }

        public bool IsExecutableFile()
        {
            byte[] result = new byte[2];
            Array.Copy(FileContent, 0, result, 0, 2);
            return result[0] == 239 && result[1] == 187;
        }
    }
}