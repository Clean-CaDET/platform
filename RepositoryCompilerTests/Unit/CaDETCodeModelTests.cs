using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using Shouldly;
using System;
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
        public void Calculates_lines_of_code_for_CSharp_class_elements()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetDoctorClassText());

            var doctorClass = classes.First();
            doctorClass.Metrics.LOC.ShouldBe(22);
            doctorClass.FindMember("Email").Metrics.LOC.ShouldBe(1);
            doctorClass.FindMember("IsAvailable").Metrics.LOC.ShouldBe(8);
        }

        [Fact]
        public void Calculates_method_cyclomatic_complexity()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();

            gitClass.FindMember("CheckoutCommit").Metrics.CYCLO.ShouldBe(2);
            gitClass.FindMember("ParseDocuments").Metrics.CYCLO.ShouldBe(4);
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
            var overlapsWith = dateRange.FindMember("OverlapsWith");
            var logChecked = service.FindMember("LogChecked");
            var findDoctors = service.FindMember("FindAvailableDoctor");
            findDoctors.InvokedMethods.ShouldContain(overlapsWith);
            findDoctors.InvokedMethods.ShouldContain(logChecked);
        }

        [Fact]
        public void Checks_method_signature()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetMultipleClassTexts());

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
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetMultipleClassTexts());

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var holidayDates = doctor.FindMember("HolidayDates");
            var findDoctors = service.FindMember("FindAvailableDoctor");
            findDoctors.AccessedAccessors.ShouldContain(holidayDates);
            findDoctors.AccessedFields.ShouldContain(doctor.Fields.Find(f => f.Name.Equals("Test")));
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
        public void Calculates_tight_class_cohesion()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetTCCMultipleClassTexts());

            var class6 = classes.Find(c => c.Name.Equals("Class6"));
            var class7 = classes.Find(c => c.Name.Equals("Class7"));
            var class8 = classes.Find(c => c.Name.Equals("Class8"));

            class6.Metrics.TCC.ShouldBe(0.67);
            class7.Metrics.TCC.ShouldBe(0.5);
            class8.Metrics.TCC.ShouldBe(0.5);
        }

        [Fact]
        public void Calculates_access_to_foreign_data()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetATFDMultipleClassTexts());

            var class1 = classes.Find(c => c.Name.Equals("Class1"));
            var class3 = classes.Find(c => c.Name.Equals("Class3"));
            var class5 = classes.Find(c => c.Name.Equals("Class3"));

            class1.Metrics.ATFD.ShouldBe(2);
            class3.Metrics.ATFD.ShouldBe(1);
            class5.Metrics.ATFD.ShouldBe(1);
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
        public void Builds_member_parameters()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetMultipleClassTexts());

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
        public void Calculates_number_of_parameters()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.FindMember("CheckForNewCommits").Metrics.NOP.ShouldBe(0);
            gitClass.FindMember("PullChanges").Metrics.NOP.ShouldBe(0);
            gitClass.FindMember("GetCommits").Metrics.NOP.ShouldBe(1);
            gitClass.FindMember("CheckoutCommit").Metrics.NOP.ShouldBe(1);
        }

        [Fact]
        public void Calculates_number_of_local_variables()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            List<CaDETClass> classes = builder.BuildCodeModel(_testDataFactory.GetGitAdapterClassText());

            var gitClass = classes.First();
            gitClass.FindMember("CheckForNewCommits").Metrics.NOLV.ShouldBe(2);
            gitClass.FindMember("GetActiveCommit").Metrics.NOLV.ShouldBe(0);
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
            doctor.FindMember("Doctor").AccessedAccessors.ShouldContain(employee.FindMember("Email"));
            employee.FindMember("Employee").AccessedAccessors.ShouldContain(entity.FindMember("Id"));
        }
    }
}
