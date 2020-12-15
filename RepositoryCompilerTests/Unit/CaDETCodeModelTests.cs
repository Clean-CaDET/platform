using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompilerTests.DataFactories;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using RepositoryCompiler.CodeModel.CodeParsers.CSharp.Exceptions;
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

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetDoctorClassText());

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
        public void Checks_method_signature()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetMultipleClassTexts());

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var holidayDates = doctor.FindMember("HolidayDates");
            var overlapsWith = dateRange.FindMember("OverlapsWith");
            var findDoctors = service.FindMember("FindAvailableDoctor");
            holidayDates.Signature().Equals("HolidayDates");
            overlapsWith.Signature().Equals("OverlapsWith(DoctorApp.Model.Data.DateR.DateRange)");
            findDoctors.Signature().Equals("FindAvailableDoctor(DoctorApp.Model.Data.DateR.DateRange)");
        }

        [Fact]
        public void Calculates_invoked_methods()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetMultipleClassTexts());

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var overlapsWith = dateRange.FindMember("OverlapsWith");
            var logChecked = service.FindMember("LogChecked");
            var findDoctors = service.FindMember("FindAvailableDoctor");
            
            findDoctors.InvokedMethods.ShouldContain(overlapsWith);
            findDoctors.InvokedMethods.ShouldContain(logChecked);
            logChecked.InvokedMethods.ShouldContain(findDoctors);
            logChecked.InvokedMethods.ShouldContain(overlapsWith);
            logChecked.InvokedMethods.ShouldContain(doctor.FindMember("TestFunction"));
        }
        [Fact]
        public void Calculates_accessed_fields()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetMultipleClassTexts());
            
            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var holidayDates = doctor.FindMember("HolidayDates");
            var findDoctors = service.FindMember("FindAvailableDoctor");
            var logChecked = service.FindMember("LogChecked");
            
            findDoctors.AccessedFields.ShouldContain(doctor.Fields.Find(f => f.Name.Equals("Test")));
            logChecked.AccessedFields.ShouldContain(service.FindField("_doctors"));
            logChecked.AccessedFields.ShouldContain(doctor.FindField("Test"));
            logChecked.AccessedFields.ShouldContain(doctor.FindField("TestDR"));
            logChecked.AccessedFields.ShouldContain(dateRange.FindField("NumOfDays"));
        }
        [Fact]
        public void Calculates_accessed_accessors()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetMultipleClassTexts());
            
            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var holidayDates = doctor.FindMember("HolidayDates");
            var findDoctors = service.FindMember("FindAvailableDoctor");
            var logChecked = service.FindMember("LogChecked");
            
            findDoctors.AccessedAccessors.ShouldContain(holidayDates);
            logChecked.AccessedAccessors.ShouldContain(service.FindMember("TestDoc"));
            logChecked.AccessedAccessors.ShouldContain(doctor.FindMember("TestProp"));
            logChecked.AccessedAccessors.ShouldContain(doctor.FindMember("Name"));
            logChecked.AccessedAccessors.ShouldContain(dateRange.FindMember("From"));
            logChecked.AccessedAccessors.ShouldContain(dateRange.FindMember("To"));
        }
        [Fact]
        public void Calculates_array_element_accessed_accessor()
        {
            //CURRENTLY NOT SUPPORTED - ergo tests fail.
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetMultipleClassTexts());
            
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var holidayDates = doctor.FindMember("HolidayDates");
            var logChecked = service.FindMember("LogChecked");
            
            logChecked.AccessedAccessors.ShouldContain(holidayDates);
        }

        [Fact]
        public void Determines_if_is_data_class()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetMultipleClassTexts());

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            dateRange.IsDataClass().ShouldBeFalse();
            doctor.IsDataClass().ShouldBeFalse();
            service.IsDataClass().ShouldBeFalse();
        }

        [Fact]
        public void Builds_member_parameters()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetMultipleClassTexts());

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

            List<CaDETClass> classes = factory.CreateClassModel(_testDataFactory.GetClassesWithHierarchy());

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
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            Should.Throw<NonUniqueFullNameException>(() => factory.CreateClassModel(_testDataFactory.GetTwoClassesWithSameFullName()));
        }

        [Fact]
        public void Ignores_partial_classes()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            var classes = factory.CreateClassModel(_testDataFactory.GetTwoPartialClassesWithSameFullName());

            classes.Count.ShouldBe(0);
        }
    }
}
