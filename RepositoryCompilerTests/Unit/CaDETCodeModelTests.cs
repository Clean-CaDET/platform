using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
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
            doctorClass.FindMethod("Email").Metrics.LOC.ShouldBe(1);
            doctorClass.FindMethod("IsAvailable").Metrics.LOC.ShouldBe(8);
        }

        [Fact]
        public void Calculates_method_cyclomatic_complexity()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.FindMethod("CheckoutCommit").Metrics.CYCLO.ShouldBe(2);
            gitClass.FindMethod("ParseDocuments").Metrics.CYCLO.ShouldBe(4);
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
            var overlapsWith = dateRange.FindMethod("OverlapsWith");
            var logChecked = service.FindMethod("LogChecked");
            var findDoctors = service.FindMethod("FindAvailableDoctor");
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
            var holidayDates = doctor.FindMethod("HolidayDates");
            var findDoctors = service.FindMethod("FindAvailableDoctor");
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
        [Fact]
        public void Determines_if_is_data_class()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetMultipleClassTexts());

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            dateRange.IsDataClass().ShouldBeFalse();
            doctor.IsDataClass().ShouldBeTrue();
            service.IsDataClass().ShouldBeFalse();
        }
        [Fact]
        public void Establishes_correct_class_hierarchy()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetClassesWithHierarchy());

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var employee = classes.Find(c => c.Name.Equals("Employee"));
            var entity = classes.Find(c => c.Name.Equals("Entity"));
            doctor.Parent.ShouldBe(employee);
            employee.Parent.ShouldBe(entity);
            entity.Parent.ShouldBeNull();
            doctor.FindMethod("Doctor").AccessedFieldsAndAccessors.ShouldContain(employee.FindMethod("Email"));
            employee.FindMethod("Employee").AccessedFieldsAndAccessors.ShouldContain(entity.FindMethod("Id"));
        }
    }
}
