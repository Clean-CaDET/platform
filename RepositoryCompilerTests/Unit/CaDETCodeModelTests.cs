using System.Collections.Generic;
using System.Linq;
using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel;
using Shouldly;
using Xunit;

namespace RepositoryCompilerTests.Unit
{
    public class CaDETCodeModelTests
    {
        private readonly CodeModelTestDataFactory _testDataFactory = new CodeModelTestDataFactory();
        
        //This test is a safety net for the C# SyntaxParser and serves to check
        //the understanding of the API. It should probably be removed in the long run.
        [Fact]
        public void Compiles_CSharp_class_with_appropriate_fields_and_methods()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetDoctorClassText());

            classes.ShouldHaveSingleItem();
            var doctorClass = classes.First();
            doctorClass.MetricNAD().ShouldBe(0);
            doctorClass.MetricNMD().ShouldBe(5);
            doctorClass.Methods.ShouldContain(method => method.IsAccessor && method.Name.Equals("Email"));
            doctorClass.Methods.ShouldContain(method => method.IsConstructor);
            doctorClass.Methods.ShouldContain(method =>
                !method.IsConstructor && !method.IsAccessor && method.Name.Equals("IsAvailable"));
            doctorClass.Methods.First().Parent.SourceCode.ShouldBe(doctorClass.SourceCode);
        }
        [Fact]
        public void Calculates_lines_of_code_for_CSharp_class_elements()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetDoctorClassText());
            
            var doctorClass = classes.First();
            doctorClass.MetricLOC().ShouldBe(22);
            doctorClass.Methods.Find(method => method.Name.Equals("Email")).MetricLOC().ShouldBe(1);
            doctorClass.Methods.Find(method => method.Name.Equals("IsAvailable")).MetricLOC().ShouldBe(8);
        }
        [Fact]
        public void Calculates_method_cyclomatic_complexity()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetGitAdapterClassText());
            
            var gitClass = classes.First();
            gitClass.Methods.Find(method => method.Name.Equals("CheckoutCommit")).MetricCYCLO.ShouldBe(2);
            gitClass.Methods.Find(method => method.Name.Equals("ParseDocuments")).MetricCYCLO.ShouldBe(4);
        }
        [Fact]
        public void Calculates_weighted_methods_per_class()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.MetricWMC().ShouldBe(17);
        }
    }
}
