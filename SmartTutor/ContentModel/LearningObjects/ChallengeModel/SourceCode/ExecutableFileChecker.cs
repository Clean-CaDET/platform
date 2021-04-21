using System;
using System.IO;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.SourceCode
{
    public class ExecutableFileChecker
    {
        private readonly byte[] _fileContent;
        private readonly byte[] EXE_SIGNATURE = { 239, 187 };

        public ExecutableFileChecker()
        {
            var filePath = @".\ContentModel\LearningObjects\ChallengeModel\SourceCode\FileStorage\SourceCode.cs";
            _fileContent = File.ReadAllBytes(filePath);
        }

        public bool IsExecutableFile()
        {
            byte[] result = new byte[2];
            Array.Copy(_fileContent, 0, result, 0, 2);
            return result.SequenceEqual(EXE_SIGNATURE);
        }
    }
}