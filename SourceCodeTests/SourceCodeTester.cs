using System.IO;

namespace FileTests
{
    public class SourceCodeTester
    {
        private readonly ExecutableFileTests _executableFileTests = new ExecutableFileTests();

        public bool ValidSourceCode(string[] sourceCode)
        {
            CreateFileFromCode(sourceCode);
            _executableFileTests.Check_If_Executable_File();
            return _executableFileTests.IsExecutable;
        }

        private void CreateFileFromCode(string[] sourceCode)
        {
            string path = @"..\SourceCodeTests\FileStorage\SourceCodeFile.txt";
            foreach (var line in sourceCode)
                File.WriteAllText(path, line);
        }
    }
}