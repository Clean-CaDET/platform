using Shouldly;
using System;
using System.IO;
using Xunit;

namespace SmartTutorTests.Unit
{
    public class SourceCodeTests
    {
        private readonly byte[] _fileContent;

        public SourceCodeTests()
        {
            _fileContent = File.ReadAllBytes(@"..\..\..\FileStorage\SourceCode.cs");
        }

        [Fact]
        public void Check_If_Executable_File()
        {
            byte[] result = new byte[2];

            Array.Copy(_fileContent, 0, result, 0, 2);

            result.Length.ShouldBe(2);
            result[0].ShouldBe(Convert.ToByte(239));
            result[1].ShouldBe(Convert.ToByte(187));
        }
    }
}
