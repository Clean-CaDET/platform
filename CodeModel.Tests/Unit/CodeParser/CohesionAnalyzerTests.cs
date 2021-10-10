using System;
using System.Collections.Generic;
using System.IO;
using CodeModel.CaDETModel.CodeItems;
using CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer;
using CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer.Metrics;
using CodeModel.CodeParsers.CSharp.Exceptions;
using Xunit;

namespace CodeModel.Tests.Unit.CodeParser
{
    public class CohesionAnalyzerTests
    {
        [Fact]
        public void Test_Class_Without_Data_Members()
        {
            string[] classCode = GetCode("CohesionAnalyzer/ClassWithoutDataMembers.txt");
            CaDETClass testClass = new CodeModelFactory().CreateProject(classCode).Classes[0];

            var analyzer = new CohesionAnalyzer(new Coh());
            Exception ex = Assert.Throws<ClassWithoutElementsException>(() =>
                analyzer.IdentifyCohesiveParts(testClass));
            Assert.Equal("Class `CohesionAnalyzer` has no data members.", ex.Message);
        }

        [Fact]
        public void Test_Class_Without_Normal_Methods()
        {
            string[] classCode = GetCode("CohesionAnalyzer/ClassWithoutMethods.txt");
            CaDETClass testClass = new CodeModelFactory().CreateProject(classCode).Classes[0];

            var analyzer = new CohesionAnalyzer(new Coh());
            Exception ex = Assert.Throws<ClassWithoutElementsException>(() =>
                analyzer.IdentifyCohesiveParts(testClass));
            Assert.Equal("Class `CohesionAnalyzer` has no normal methods.", ex.Message);
        }

        [Theory]
        [MemberData(nameof(GetTestClassesWithSingleResult))]
        public void Test_Identify_Cohesive_Parts_Within_A_Class(string classPath,
            string[] expectedCohesiveParts)
        {
            string[] classCode = GetCode(classPath);
            CaDETClass testClass = new CodeModelFactory().CreateProject(classCode).Classes[0];

            CohesionAnalyzer analyzer = new CohesionAnalyzer(new Coh());
            var results = analyzer.IdentifyCohesiveParts(testClass);

            Assert.Equal(expectedCohesiveParts, results);
        }

        public static IEnumerable<object[]> GetTestClassesWithSingleResult() => new List<object[]>
        {
            new object[]
            {
                @"CohesionAnalyzer/ClassInteractions.txt",
                new[]
                {
                    "To perform the refactoring remove the following method-field accesses:\n" +
                    "GetInvalidEdgeGroups -> MethodFieldAccessMapping\n" +
                    "Cohesive part:\nData members: MethodFieldAccessMapping\nNormal methods: GetAllInteractionEdges\n" +
                    "Cohesive part:\nData members: Edges\nNormal methods: GetInvalidEdgeGroups"
                }
            },

            new object[]
            {
                @"CohesionAnalyzer/DisconnectedClass.txt",
                new[]
                {
                    "Class is already disconnected. No accesses should be removed.\n" +
                    "Cohesive part:\nData members: a\nNormal methods: getA\n" +
                    "Cohesive part:\nData members: b, c\nNormal methods: BPlusC, BMinusC"
                }
            },

            new object[]
            {
                @"CohesionAnalyzer/SurfPhysics.txt",
                new[]
                {
                    "To perform the refactoring remove the following method-field accesses:\nReflect -> groundLayerMask\n" +
                    "Cohesive part:\nData members: groundLayerMask, _colliders\nNormal methods: ResolveCollisions, StepOffset\n" +
                    "Cohesive part:\nData members: _planes, maxClipPlanes, numBumps, SurfSlope\nNormal methods: Reflect"
                }
            },

            new object[]
            {
                @"CohesionAnalyzer/UserController.txt",
                new[]
                {
                    "To perform the refactoring remove the following method-field accesses:\nRegisterUser -> _userManager\nGetAllUser -> _logger\nGetUserList -> _logger\nLogin -> _logger\n" +
                    "Cohesive part:\nData members: _logger, _rolewManager, _jWTConfig\nNormal methods: RegisterUser, AddRole, GetRoles, GenerateToken\n" +
                    "Cohesive part:\nData members: _userManager, _signInManager\nNormal methods: GetAllUser, GetUserList, Login",
                    "To perform the refactoring remove the following method-field accesses:\nRegisterUser -> _logger\nRegisterUser -> _rolewManager\nGetAllUser -> _logger\nGetUserList -> _logger\nLogin -> _logger\n" +
                    "Cohesive part:\nData members: _userManager, _signInManager\nNormal methods: RegisterUser, GetAllUser, GetUserList, Login\n" +
                    "Cohesive part:\nData members: _logger, _rolewManager, _jWTConfig\nNormal methods: AddRole, GetRoles, GenerateToken"
                }
            },

            new object[]
            {
                @"CohesionAnalyzer/FullyConnectedClass.txt",
                Array.Empty<object>()
            },

            new object[]
            {
                @"CohesionAnalyzer/TestClass3.txt",
                Array.Empty<object>()
            },

            new object[]
            {
                @"CohesionAnalyzer/HardLinkHelper.txt",
                new[]
                {
                    "Class is already disconnected. No accesses should be removed.\n" +
                    "Cohesive part:\nData members: _builder\nNormal methods: HardLink, HardLink, CreateHarkLink, Copy, CreateFolder\n" +
                    "Cohesive part:\nData members: _createdFolders\nNormal methods: SearchFolder"
                }
            }
        };

        private string[] GetCode(string path)
        {
            string classCode = File.ReadAllText("../../../DataFactories/TestClasses/" + path);
            return new[]
            {
                classCode
            };
        }
    }
}