using CodeModel.CaDETModel.CodeItems;
using CodeModel.CodeParsers.CSharp.Exceptions;
using CodeModel.Tests.DataFactories;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CodeModel.Tests.Unit.CodeParser
{
    public class CaDETClassTests
    {
        private static readonly CodeFactory TestDataFactory = new();

        //This test is a safety net for the C# SyntaxParser and serves to check the understanding of the API.
        [Fact]
        public void Compiles_CSharp_class_with_appropriate_fields_and_methods()
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(TestDataFactory.GetMultipleClassTexts()).Classes;

            var doctorClass = classes.First(c => c.Name.Equals("Doctor"));
            doctorClass.Members.ShouldContain(method =>
                method.Type.Equals(CaDETMemberType.Property) && method.Name.Equals("Email"));
            doctorClass.Members.ShouldContain(method => method.Type.Equals(CaDETMemberType.Constructor));
            doctorClass.Members.ShouldContain(method =>
                method.Type.Equals(CaDETMemberType.Method) && method.Name.Equals("TestFunction"));
            doctorClass.Members.First().Parent.SourceCode.ShouldBe(doctorClass.SourceCode);
            doctorClass.FindMember("Email").Modifiers.First().Value.ShouldBe(CaDETModifierValue.Public);
            doctorClass.FindMember("TestFunction").Modifiers.First().Value.ShouldBe(CaDETModifierValue.Internal);
        }

        [Theory]
        [MemberData(nameof(CodeFactory.GetInvalidSyntaxClasses), MemberType = typeof(CodeFactory))]
        public void Checks_syntax_errors(string[] sourceCode, int syntaxErrorCount)
        {
            CodeModelFactory factory = new CodeModelFactory();

            var project = factory.CreateProject(sourceCode);

            project.SyntaxErrors.Count.ShouldBe(syntaxErrorCount);
        }

        [Fact]
        public void Fails_to_build_code_with_nonunique_class_full_names()
        {
            CodeModelFactory factory = new CodeModelFactory();

            Should.Throw<NonUniqueFullNameException>(() => factory.CreateProject(TestDataFactory.GetTwoClassesWithSameFullName()));
        }

        [Fact]
        public void Ignores_partial_classes()
        {
            CodeModelFactory factory = new CodeModelFactory();

            var classes = factory.CreateProject(TestDataFactory.GetTwoPartialClassesWithSameFullName()).Classes;

            classes.Count.ShouldBe(0);
        }

        [Fact]
        public void Checks_field_linked_types()
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(TestDataFactory.GetMultipleClassTexts()).Classes;
            List<CaDETClass> otherClasses = factory.CreateProject(TestDataFactory.GetClassesFromDifferentNamespace()).Classes;

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var doctorService = classes.Find(c => c.Name.Equals("DoctorService"));
            var otherDoctor = otherClasses.Find(c => c.Name.Equals("Doctor"));
            var otherDateRange = otherClasses.Find(c => c.Name.Equals("DateRange"));

            dateRange.FindField("testDictionary").GetLinkedTypes().ShouldContain(doctor);
            dateRange.FindField("testDictionary").GetLinkedTypes().ShouldNotContain(otherDoctor);
            dateRange.FindField("testDictionary").GetLinkedTypes().ShouldContain(doctorService);
            doctor.FindField("TestDR").GetLinkedTypes().ShouldContain(dateRange);
            doctor.FindField("TestDR").GetLinkedTypes().ShouldNotContain(otherDateRange);
            doctorService.FindField("_doctors").GetLinkedTypes().ShouldContain(doctor);
        }

        [Fact]
        public void Establishes_correct_class_hierarchy()
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(TestDataFactory.GetClassesWithHierarchy()).Classes;

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
        public void Determines_if_is_data_class()
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(TestDataFactory.GetMultipleClassTexts()).Classes;

            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var service = classes.Find(c => c.Name.Equals("DoctorService"));
            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            dateRange.IsDataClass().ShouldBeFalse();
            doctor.IsDataClass().ShouldBeFalse();
            service.IsDataClass().ShouldBeFalse();
        }

        [Fact]
        public void Checks_linked_invoked_method_types()
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(TestDataFactory.GetMultipleClassTexts()).Classes;

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var doctorService = classes.Find(c => c.Name.Equals("DoctorService"));
            var methodInvocationsTypes = doctorService.GetMethodInvocationsTypes();

            methodInvocationsTypes.ShouldContain(dateRange);
            methodInvocationsTypes.ShouldContain(doctor);
            methodInvocationsTypes.Count.ShouldBe(3);
        }

        [Fact]
        public void Checks_linked_parameter_types()
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(TestDataFactory.GetMultipleClassTexts()).Classes;

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var doctorService = classes.Find(c => c.Name.Equals("DoctorService"));
            var parameterTypes = doctorService.GetMethodLinkedParameterTypes();

            parameterTypes.ShouldContain(dateRange);
            parameterTypes.ShouldContain(doctor);
            parameterTypes.Count.ShouldBe(2);
        }

        [Fact]
        public void Checks_linked_accessed_fields_types()
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(TestDataFactory.GetMultipleClassTexts()).Classes;

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var doctorService = classes.Find(c => c.Name.Equals("DoctorService"));
            var accessedFieldTypes = doctorService.GetAccessedFieldsTypes();

            accessedFieldTypes.ShouldContain(doctor);
            accessedFieldTypes.ShouldNotContain(dateRange);
            accessedFieldTypes.Count.ShouldBe(3);
        }

        [Fact]
        public void Checks_linked_accessed_accessor_types()
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(TestDataFactory.GetMultipleClassTexts()).Classes;

            var dateRange = classes.Find(c => c.Name.Equals("DateRange"));
            var doctor = classes.Find(c => c.Name.Equals("Doctor"));
            var doctorService = classes.Find(c => c.Name.Equals("DoctorService"));
            var accessedAccessorTypes = doctorService.GetAccessedAccessorsTypes();

            accessedAccessorTypes.ShouldContain(doctor);
            accessedAccessorTypes.ShouldContain(dateRange);
            accessedAccessorTypes.Count.ShouldBe(6);
        }
    }
}
