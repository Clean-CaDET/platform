using System.Collections.Generic;
using System.Linq;
using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompilerTests.DataFactories;
using Shouldly;
using Xunit;

namespace RepositoryCompilerTests.Unit
{
    public class MetricCalculationTests
    {
        private readonly CodeFactory _testDataFactory = new CodeFactory();

        [Fact]
        public void Calculates_lines_of_code_for_CSharp_class_elements()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetDoctorClassText());

            var doctorClass = classes.First();
            doctorClass.Metrics.LOC.ShouldBe(22);
            doctorClass.FindMember("Email").Metrics.LOC.ShouldBe(1);
            doctorClass.FindMember("IsAvailable").Metrics.LOC.ShouldBe(8);
        }
        
        [Fact]
        public void Calculates_weighted_methods_per_class()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.Metrics.WMC.ShouldBe(17);
        }


        [Fact]
        public void Calculates_access_to_foreign_data()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetATFDMultipleClassTexts());

            var class1 = classes.Find(c => c.Name.Equals("Class1"));
            var class3 = classes.Find(c => c.Name.Equals("Class3"));
            var class5 = classes.Find(c => c.Name.Equals("Class5"));
            var class7 = classes.Find(c => c.Name.Equals("Class7"));
            var class9 = classes.Find(c => c.Name.Equals("Class9"));

            class1.Metrics.ATFD.ShouldBe(2);
            class3.Metrics.ATFD.ShouldBe(1);
            class5.Metrics.ATFD.ShouldBe(1);
            class7.Metrics.ATFD.ShouldBe(3);
            class9.Metrics.ATFD.ShouldBe(2);
        }

        [Fact]
        public void Calculates_lack_of_cohesion()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetCohesionClasses());

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            dateRange.Metrics.LCOM.ShouldBe(0);
            doctor.Metrics.LCOM.ShouldBe(0.75);
        }

        [Fact]
        public void Calculates_tight_class_cohesion()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetTCCMultipleClassTexts());

            var class6 = classes.Find(c => c.Name.Equals("Class6"));
            var class7 = classes.Find(c => c.Name.Equals("Class7"));
            var class8 = classes.Find(c => c.Name.Equals("Class8"));

            class6.Metrics.TCC.ShouldBe(0.67);
            class7.Metrics.TCC.ShouldBe(0.5);
            class8.Metrics.TCC.ShouldBe(0.5);
        }
        
        [Fact]
        public void Calculates_method_cyclomatic_complexity()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();

            gitClass.FindMember("CheckoutCommit").Metrics.CYCLO.ShouldBe(2);
            gitClass.FindMember("ParseDocuments").Metrics.CYCLO.ShouldBe(4);
        }


        [Fact]
        public void Calculates_number_of_parameters()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.FindMember("CheckForNewCommits").Metrics.NOP.ShouldBe(0);
            gitClass.FindMember("PullChanges").Metrics.NOP.ShouldBe(0);
            gitClass.FindMember("GetCommits").Metrics.NOP.ShouldBe(1);
            gitClass.FindMember("CheckoutCommit").Metrics.NOP.ShouldBe(1);
        }

        [Fact]
        public void Calculates_number_of_local_variables()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.FindMember("CheckForNewCommits").Metrics.NOLV.ShouldBe(2);
            gitClass.FindMember("GetActiveCommit").Metrics.NOLV.ShouldBe(0);
        }
    }
}
