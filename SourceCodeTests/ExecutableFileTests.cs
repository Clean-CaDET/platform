using Shouldly;
using System;
using System.IO;
using Xunit;

namespace FileTests
{
    public class ExecutableFileTests
    {
        public byte[] FileContent { get; set; }

        public ExecutableFileTests()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\SourceCodeTests\\FileStorage\\SourceCodeFile.txt");
            FileContent = File.ReadAllBytes(filePath);
        }

        [Fact]
        public void Check_If_Executable_File()
        {
            byte[] result = new byte[2];
            Array.Copy(FileContent, 0, result, 0, 2);

            try
            {
                result.Length.ShouldBe(2);
                result[0].ShouldBe(Convert.ToByte(239));
                result[1].ShouldBe(Convert.ToByte(187));
            }
            catch (Exception)
            {
                throw new Exception("Code can not be executed!");
            }
        }
    }
}