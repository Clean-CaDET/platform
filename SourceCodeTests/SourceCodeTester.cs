using System.IO;

namespace FileTests
{
    public class SourceCodeTester
    {
        public static void ValidateSourceCode(string[] sourceCode)
        {
            CreateFileFromCode(sourceCode);
            new ExecutableFileTests().Check_If_Executable_File();
        }

        private static void CreateFileFromCode(string[] sourceCode)
        {
            string path = @"..\SourceCodeTests\FileStorage\SourceCodeFile.txt";
            foreach (var line in sourceCode)
                File.WriteAllText(path, line);
        }
    }
}