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
            doctorClass.Metrics.NAD.ShouldBe(0);
            doctorClass.Metrics.NMD.ShouldBe(1);
            doctorClass.Methods.ShouldContain(method =>
                method.Type.Equals(CaDETMemberType.Property) && method.Name.Equals("Email"));
            doctorClass.Methods.ShouldContain(method => method.Type.Equals(CaDETMemberType.Constructor));
            doctorClass.Methods.ShouldContain(method =>
                method.Type.Equals(CaDETMemberType.Method) && method.Name.Equals("IsAvailable"));
            doctorClass.Methods.First().Parent.SourceCode.ShouldBe(doctorClass.SourceCode);
        }

        [Fact]
        public void Calculates_lines_of_code_for_CSharp_class_elements()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetDoctorClassText());

            var doctorClass = classes.First();
            doctorClass.Metrics.LOC.ShouldBe(22);
            doctorClass.Methods.Find(method => method.Name.Equals("Email")).Metrics.LOC.ShouldBe(1);
            doctorClass.Methods.Find(method => method.Name.Equals("IsAvailable")).Metrics.LOC.ShouldBe(8);
        }

        [Fact]
        public void Calculates_method_cyclomatic_complexity()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.Methods.Find(method => method.Name.Equals("CheckoutCommit")).Metrics.CYCLO.ShouldBe(2);
            gitClass.Methods.Find(method => method.Name.Equals("ParseDocuments")).Metrics.CYCLO.ShouldBe(4);
        }

        [Fact]
        public void Calculates_weighted_methods_per_class()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.Metrics.WMC.ShouldBe(17);
        }

        [Fact]
        public void Calculates_invoked_methods()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetMultipleClassTexts());

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var overlapsWith = dateRange.Methods.Find(m => m.Name.Equals("OverlapsWith"));
            var logChecked = service.Methods.Find(m => m.Name.Equals("LogChecked"));
            var findDoctors = service.Methods.Find(m => m.Name.Equals("FindAvailableDoctor"));
            findDoctors.InvokedMethods.ShouldContain(overlapsWith);
            findDoctors.InvokedMethods.ShouldContain(logChecked);
        }

        [Fact]
        public void Calculates_accessed_fields()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetMultipleClassTexts());

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var holidayDates = doctor.Methods.Find(m =>
                m.Name.Equals("HolidayDates") && m.Type.Equals(CaDETMemberType.Property));
            var findDoctors = service.Methods.Find(m => m.Name.Equals("FindAvailableDoctor"));
            findDoctors.AccessedFieldsAndAccessors.ShouldContain(holidayDates);
        }

        [Fact]
        public void Calculates_lack_of_cohesion()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetCohesionClasses());

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            dateRange.Metrics.LCOM.ShouldBe(0);
            doctor.Metrics.LCOM.ShouldBe(0.75);
        }
    }
}
