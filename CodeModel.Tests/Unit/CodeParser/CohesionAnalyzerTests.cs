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
            CohesivePartsOutput[] expectedCohesiveParts)
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
                    new CohesivePartsOutput(
                        "To perform refactoring remove following method-field accesses:\nMethod: GetInvalidEdgeGroups -> Field: MethodFieldAccessMapping\n",
                        new List<string>
                        {
                            "Cohesive part:\nFields & Accessors: MethodFieldAccessMapping\nNormal methods: GetAllInteractionEdges",
                            "Cohesive part:\nFields & Accessors: Edges\nNormal methods: GetInvalidEdgeGroups"
                        })
                }
            },

            new object[]
            {
                @"CohesionAnalyzer/DisconnectedClass.txt",
                new[]
                {
                    new CohesivePartsOutput("Class is already disconnected. No accesses should be removed.\n",
                        new List<string>
                        {
                            "Cohesive part:\nFields & Accessors: a\nNormal methods: getA",
                            "Cohesive part:\nFields & Accessors: b, c\nNormal methods: BPlusC, BMinusC"
                        })
                }
            },

            new object[]
            {
                @"CohesionAnalyzer/SurfPhysics.txt",
                new[]
                {
                    new CohesivePartsOutput(
                        "To perform refactoring remove following method-field accesses:\nMethod: Reflect -> Field: groundLayerMask\n",
                        new List<string>
                        {
                            "Cohesive part:\nFields & Accessors: groundLayerMask, _colliders\nNormal methods: ResolveCollisions, StepOffset",
                            "Cohesive part:\nFields & Accessors: _planes, maxClipPlanes, numBumps, SurfSlope\nNormal methods: Reflect"
                        })
                }
            },

            new object[]
            {
                @"CohesionAnalyzer/UserController.txt",
                new[]
                {
                    new CohesivePartsOutput(
                        "To perform refactoring remove following method-field accesses:\nMethod: RegisterUser -> Field: _userManager\nMethod: GetAllUser -> Field: _logger\nMethod: GetUserList -> Field: _logger\nMethod: Login -> Field: _logger\n",
                        new List<string>
                        {
                            "Cohesive part:\nFields & Accessors: _logger, _rolewManager, _jWTConfig\nNormal methods: RegisterUser, AddRole, GetRoles, GenerateToken",
                            "Cohesive part:\nFields & Accessors: _userManager, _signInManager\nNormal methods: GetAllUser, GetUserList, Login"
                        }),
                    new CohesivePartsOutput(
                        "To perform refactoring remove following method-field accesses:\nMethod: RegisterUser -> Field: _logger\nMethod: RegisterUser -> Field: _rolewManager\nMethod: GetAllUser -> Field: _logger\nMethod: GetUserList -> Field: _logger\nMethod: Login -> Field: _logger\n",
                        new List<string>
                        {
                            "Cohesive part:\nFields & Accessors: _userManager, _signInManager\nNormal methods: RegisterUser, GetAllUser, GetUserList, Login",
                            "Cohesive part:\nFields & Accessors: _logger, _rolewManager, _jWTConfig\nNormal methods: AddRole, GetRoles, GenerateToken"
                        })
                }
            },

            new object[]
            {
                @"CohesionAnalyzer/FullyConnectedClass.txt",
                Array.Empty<object>()
            },

            new object[]
            {
                @"CohesionAnalyzer/HardLinkHelper.txt",
                new[]
                {
                    new CohesivePartsOutput("Class is already disconnected. No accesses should be removed.\n",
                        new List<string>
                        {
                            "Cohesive part:\nFields & Accessors: _builder\nNormal methods: HardLink, HardLink, CreateHarkLink, Copy, CreateFolder",
                            "Cohesive part:\nFields & Accessors: _createdFolders\nNormal methods: SearchFolder"
                        })
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