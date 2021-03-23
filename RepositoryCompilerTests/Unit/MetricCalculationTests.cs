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

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetDoctorClassText());

            var doctorClass = classes.First();
            doctorClass.Metrics.LOC.ShouldBe(22);
            doctorClass.FindMember("Email").Metrics.LOC.ShouldBe(1);
            doctorClass.FindMember("IsAvailable").Metrics.LOC.ShouldBe(8);
        }

        [Fact]
        public void Calculates_weighted_methods_per_class()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.Metrics.WMC.ShouldBe(17);
        }


        [Fact]
        public void Calculates_access_to_foreign_data()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetATFDMultipleClassTexts());

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

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCohesionClasses());

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            dateRange.Metrics.LCOM.ShouldBe(0);
            doctor.Metrics.LCOM.ShouldBe(0.75);
        }

        [Fact]
        public void Calculates_tight_class_cohesion()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetTCCMultipleClassTexts());

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

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();

            gitClass.FindMember("CheckoutCommit").Metrics.CYCLO.ShouldBe(2);
            gitClass.FindMember("ParseDocuments").Metrics.CYCLO.ShouldBe(4);
        }

        [Fact]
        public void Calculates_member_effective_lines_of_code()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetEffectiveLinesOfCodeTest());

            var doctor = classes.First();
            doctor.FindMember("Doctor").Metrics.ELOC.ShouldBe(1);
            doctor.FindMember("IsAvailable").Metrics.ELOC.ShouldBe(2);
        }

        [Fact]
        public void Calculates_number_of_parameters()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetGitAdapterClassText());

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

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.FindMember("CheckForNewCommits").Metrics.NOLV.ShouldBe(2);
            gitClass.FindMember("GetActiveCommit").Metrics.NOLV.ShouldBe(0);
        }

        [Fact]
        public void Calculates_number_of_try_catch_blocks()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCodeBlocksClass());

            var firstClass = classes.First();
            firstClass.FindMember("CSharpCodeParserInit").Metrics.NOTC.ShouldBe(0);
            firstClass.FindMember("CreateClassMemberBuilders1").Metrics.NOTC.ShouldBe(1);
            firstClass.FindMember("CreateClassMemberBuilders2").Metrics.NOTC.ShouldBe(2);
            firstClass.FindMember("CreateClassMemberBuilders3").Metrics.NOTC.ShouldBe(3);
        }

        [Fact]
        public void Calculates_number_of_loops()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCodeBlocksClass());

            var firstClass = classes.First();
            firstClass.FindMember("CSharpCodeParserInit").Metrics.NOL.ShouldBe(0);
            firstClass.FindMember("CreateClassMemberBuilders1").Metrics.NOL.ShouldBe(1);
            firstClass.FindMember("CreateClassMemberBuilders2").Metrics.NOL.ShouldBe(2);
            firstClass.FindMember("CreateClassMemberBuilders3").Metrics.NOL.ShouldBe(4);
        }

        [Fact]
        public void Calculates_number_of_return_statements()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCodeBlocksClass());

            var firstClass = classes.First();
            firstClass.FindMember("CSharpCodeParserInit").Metrics.NOR.ShouldBe(0);
            firstClass.FindMember("CreateClassMemberBuilders1").Metrics.NOR.ShouldBe(1);
            firstClass.FindMember("CreateClassMemberBuilders2").Metrics.NOR.ShouldBe(2);
        }

        [Fact]
        public void Calculates_number_of_comparisons()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCodeBlocksClass());

            var firstClass = classes.First();
            firstClass.FindMember("CSharpCodeParserInit").Metrics.NOC.ShouldBe(0);
            firstClass.FindMember("CreateClassMemberBuilders2").Metrics.NOC.ShouldBe(4);
            firstClass.FindMember("CreateClassMemberBuilders3").Metrics.NOC.ShouldBe(3);
        }

        [Fact]
        public void Calculates_number_of_method_invocations()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCodeBlocksClass());

            var firstClass = classes.First();
            firstClass.FindMember("CSharpCodeParserInit").Metrics.NOMI.ShouldBe(0);
            firstClass.FindMember("CreateClassMemberBuilders1").Metrics.NOMI.ShouldBe(2);
            firstClass.FindMember("CreateClassMemberBuilders2").Metrics.NOMI.ShouldBe(5);
        }

        [Fact]
        public void Calculates_number_of_unique_method_invocations()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCodeBlocksClass());

            var firstClass = classes.First();
            firstClass.FindMember("CSharpCodeParserInit").Metrics.RFC.ShouldBe(0);
            firstClass.FindMember("CreateClassMemberBuilders1").Metrics.RFC.ShouldBe(2);
            firstClass.FindMember("CreateClassMemberBuilders2").Metrics.RFC.ShouldBe(3);
        }

        [Fact]
        public void Calculates_number_of_assignments()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCodeBlocksClass());

            var firstClass = classes.First();
            firstClass.FindMember("CSharpCodeParserInit").Metrics.NOA.ShouldBe(2);
            firstClass.FindMember("CreateClassMemberBuilders1").Metrics.NOA.ShouldBe(0);
        }

        [Fact]
        public void Calculates_number_of_numeric_literals()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCodeBlocksClass());

            var firstClass = classes.First();
            firstClass.FindMember("CSharpCodeParserInit").Metrics.NONL.ShouldBe(4);
            firstClass.FindMember("CreateClassMemberBuilders1").Metrics.NONL.ShouldBe(0);
            firstClass.FindMember("CreateClassMemberBuilders2").Metrics.NONL.ShouldBe(12);
        }

        [Fact]
        public void Calculates_number_of_string_literals()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCodeBlocksClass());

            var firstClass = classes.First();
            firstClass.FindMember("CSharpCodeParserInit").Metrics.NOSL.ShouldBe(1);
            firstClass.FindMember("CreateClassMemberBuilders1").Metrics.NOSL.ShouldBe(0);
            firstClass.FindMember("CreateClassMemberBuilders2").Metrics.NOSL.ShouldBe(2);
        }

        [Fact]
        public void Calculates_number_of_math_operations()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetCodeBlocksClass());

            var firstClass = classes.First();
            firstClass.FindMember("CSharpCodeParserInit").Metrics.NOMO.ShouldBe(2);
            firstClass.FindMember("CreateClassMemberBuilders1").Metrics.NOMO.ShouldBe(0);
            firstClass.FindMember("CreateClassMemberBuilders2").Metrics.NOMO.ShouldBe(6);
        }
    }
}
