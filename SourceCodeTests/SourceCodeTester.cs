using System.IO;

namespace SourceCodeTests
{
    public class SourceCodeTester
    {
        public bool ValidSourceCode(string[] sourceCode)
        {
            string file = GetFileFromCode(sourceCode);
            bool validSourceCode = new ExecutableFileTests(file).Check_If_Executable_File_Tests_Have_Passed();
            return validSourceCode;
        }

        private string GetFileFromCode(string[] sourceCode)
        {
            string path = @"..\SourceCodeTests\DataFactories\ExecutableFile.txt";
            foreach (var line in sourceCode)
                File.WriteAllText(path, line);
            return path;
        }
    }
}