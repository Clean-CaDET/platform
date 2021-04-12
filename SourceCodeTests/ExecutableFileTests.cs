using NUnit.Framework;
using Shouldly;
using System;
using System.IO;
using Xunit;

namespace SourceCodeTests
{
    public class ExecutableFileTests
    {
        public byte[] FileContent { get; set; }

        public ExecutableFileTests(string file)
        {
            FileContent = File.ReadAllBytes(file);
        }

        [TearDown]
        public bool Check_If_Executable_File_Tests_Have_Passed()
        {
            Check_If_Executable_File();
            // TODO: finish 
            return TestContext.CurrentContext.Result.Outcome.ToString().Equals("Passed");
        }

        [Fact]
        public void Check_If_Executable_File()
        {
            byte[] result = new byte[2];

            Array.Copy(FileContent, 0, result, 0, 2);

            result.Length.ShouldBe(2);
            result[0].ShouldBe(Convert.ToByte(239));
            result[1].ShouldBe(Convert.ToByte(187));
        }
    }
}