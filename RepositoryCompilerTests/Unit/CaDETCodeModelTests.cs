using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompilerTests.DataFactories;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RepositoryCompilerTests.Unit
{
    public class CaDETCodeModelTests
    {
        private readonly CodeFactory _testDataFactory = new CodeFactory();

        //This test is a safety net for the C# SyntaxParser and serves to check
        //the understanding of the API. It should probably be removed in the long run.
        [Fact]
        public void Compiles_CSharp_class_with_appropriate_fields_and_methods()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetDoctorClassText());

            classes.ShouldHaveSingleItem();
            var doctorClass = classes.First();
            doctorClass.Metrics.NAD.ShouldBe(0);
            doctorClass.Metrics.NMD.ShouldBe(1);
            doctorClass.Members.ShouldContain(method =>
                method.Type.Equals(CaDETMemberType.Property) && method.Name.Equals("Email"));
            doctorClass.Members.ShouldContain(method => method.Type.Equals(CaDETMemberType.Constructor));
            doctorClass.Members.ShouldContain(method =>
                method.Type.Equals(CaDETMemberType.Method) && method.Name.Equals("IsAvailable"));
            doctorClass.Members.First().Parent.SourceCode.ShouldBe(doctorClass.SourceCode);
            doctorClass.FindMember("Email").Modifiers.First().Value.ShouldBe(CaDETModifierValue.Public);
            doctorClass.FindMember("IsAvailable").Modifiers.First().Value.ShouldBe(CaDETModifierValue.Internal);
        }

        [Fact]
        public void Calculates_invoked_methods()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetMultipleClassTexts());

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var overlapsWith = dateRange.FindMember("OverlapsWith");
            var logChecked = service.FindMember("LogChecked");
            var findDoctors = service.FindMember("FindAvailableDoctor");
            findDoctors.InvokedMethods.ShouldContain(overlapsWith);
            findDoctors.InvokedMethods.ShouldContain(logChecked);
        }

        [Fact]
        public void Checks_method_signature()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetMultipleClassTexts());

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var holidayDates = doctor.FindMember("HolidayDates");
            var overlapsWith = dateRange.FindMember("OverlapsWith");
            var findDoctors = service.FindMember("FindAvailableDoctor");
            holidayDates.GetSignature().Equals("HolidayDates");
            overlapsWith.GetSignature().Equals("OverlapsWith(DoctorApp.Model.Data.DateR.DateRange)");
            findDoctors.GetSignature().Equals("FindAvailableDoctor(DoctorApp.Model.Data.DateR.DateRange)");
        }

        [Fact]
        public void Calculates_accessed_fields()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetMultipleClassTexts());

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var holidayDates = doctor.FindMember("HolidayDates");
            var findDoctors = service.FindMember("FindAvailableDoctor");
            findDoctors.AccessedAccessors.ShouldContain(holidayDates);
            findDoctors.AccessedFields.ShouldContain(doctor.Fields.Find(f => f.Name.Equals("Test")));
        }

        [Fact]
        public void Determines_if_is_data_class()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetMultipleClassTexts());

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            dateRange.IsDataClass().ShouldBeFalse();
            doctor.IsDataClass().ShouldBeTrue();
            service.IsDataClass().ShouldBeFalse();
        }

        [Fact]
        public void Builds_member_parameters()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetMultipleClassTexts());

            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var overlapTimeSpanParam = dateRange.FindMember("OverlapsWith").Params.First();
            overlapTimeSpanParam.Name.ShouldBe("timeSpan");
            overlapTimeSpanParam.Type.ShouldBe("DoctorApp.Model.Data.DateR.DateRange");
            var serviceTimeSpanParam = service.FindMember("FindAvailableDoctor").Params.First();
            serviceTimeSpanParam.Name.ShouldBe("timeSpan");
            serviceTimeSpanParam.Type.ShouldBe("DoctorApp.Model.Data.DateR.DateRange");
            var logParam = service.FindMember("LogChecked").Params.First();
            logParam.Name.ShouldBe("testData");
            logParam.Type.ShouldBe("int");
        }

        [Fact]
        public void Establishes_correct_class_hierarchy()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateCodeModel(_testDataFactory.GetClassesWithHierarchy());

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var employee = classes.Find(c => c.Name.Equals("Employee"));
            var entity = classes.Find(c => c.Name.Equals("Entity"));
            doctor.Parent.ShouldBe(employee);
            employee.Parent.ShouldBe(entity);
            entity.Parent.ShouldBeNull();
            doctor.FindMember("Doctor").AccessedAccessors.ShouldContain(employee.FindMember("Email"));
            employee.FindMember("Employee").AccessedAccessors.ShouldContain(entity.FindMember("Id"));
        }

        [Fact]
        public void Fails_to_build_code_with_nonunique_class_full_names()
        {
            //TODO: Make test
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            Should.Throw<KeyNotFoundException>(() => factory.CreateCodeModel(_testDataFactory.GetTwoClassesWithSameFullName()));
        }

        [Fact]
        public void Merges_partial_classes_with_same_full_name_into_single_CaDETClass()
        {
            //TODO: Test
        }
    }
}
