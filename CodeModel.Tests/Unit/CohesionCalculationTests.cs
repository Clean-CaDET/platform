using CodeModel.CaDETModel.CodeItems;
using CodeModel.Tests.DataFactories;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace CodeModel.Tests.Unit
{
    public class CohesionCalculationTests
    {
        private static readonly CodeFactory _testDataFactory = new CodeFactory();

        [Theory]
        [MemberData(nameof(LCOMTest))]
        public void Calculates_lack_of_cohesion(IEnumerable<string> classCode, string className, double lcomMetric)
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(classCode).Classes;

            var classToEvaluate = classes.Find(c => c.Name.Equals(className));
            classToEvaluate.Metrics[CaDETMetric.LCOM].ShouldBe(lcomMetric);
        }

        [Theory]
        [MemberData(nameof(TCCTest))]
        public void Calculates_tight_class_cohesion(IEnumerable<string> classCode, string className, double tccMetric)
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(classCode).Classes;

            var classToEvaluate = classes.Find(c => c.Name.Equals(className));
            classToEvaluate.Metrics[CaDETMetric.TCC].ShouldBe(tccMetric);
        }

        [Theory]
        [MemberData(nameof(LCOM3Test))]
        public void Calculates_lack_of_cohesion_3(IEnumerable<string> classCode, string className, double lcom3Metric)
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(classCode).Classes;

            var classToEvaluate = classes.Find(c => c.Name.Equals(className));
            classToEvaluate.Metrics[CaDETMetric.LCOM3].ShouldBe(lcom3Metric);
        }

        [Theory]
        [MemberData(nameof(LCOM4Test))]
        public void Calculates_lack_of_cohesion_4(IEnumerable<string> classCode, string className, double lcom4Metric)
        {
            CodeModelFactory factory = new CodeModelFactory();

            List<CaDETClass> classes = factory.CreateProject(classCode).Classes;

            var classToEvaluate = classes.Find(c => c.Name.Equals(className));
            classToEvaluate.Metrics[CaDETMetric.LCOM4].ShouldBe(lcom4Metric);
        }

        public static IEnumerable<object[]> LCOMTest =>

            new List<object[]>
            {
                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/Level.txt"),
                    "Level",
                    0.928
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteReader.txt"),
                    "AsepriteReader",
                    0
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteFile.txt"),
                    "AsepriteFile",
                    0.889
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteWriter.txt"),
                    "AsepriteWriter",
                    -1
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AudioWriter.txt"),
                    "AudioWriter",
                    -1
                }
            };

        public static IEnumerable<object[]> TCCTest =>

            new List<object[]>
            {
                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/Level.txt"),
                    "Level",
                    0.17
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteReader.txt"),
                    "AsepriteReader",
                    -1
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteFile.txt"),
                    "AsepriteFile",
                    -1
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteWriter.txt"),
                    "AsepriteWriter",
                    0
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AudioWriter.txt"),
                    "AudioWriter",
                    0
                }
            };

        public static IEnumerable<object[]> LCOM3Test =>

            new List<object[]>
            {
                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/Level.txt"),
                    "Level",
                    0.941
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteReader.txt"),
                    "AsepriteReader",
                    0
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteFile.txt"),
                    "AsepriteFile",
                    0
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteWriter.txt"),
                    "AsepriteWriter",
                    0
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AudioWriter.txt"),
                    "AudioWriter",
                    0
                }
            };

        public static IEnumerable<object[]> LCOM4Test =>

            new List<object[]>
            {
                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/Level.txt"),
                    "Level",
                    9
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteReader.txt"),
                    "AsepriteReader",
                    1
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteFile.txt"),
                    "AsepriteFile",
                    1
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AsepriteWriter.txt"),
                    "AsepriteWriter",
                    0
                },

                new object[]
                {
                    _testDataFactory.ReadClassFromFile("../../../DataFactories/TestClasses/SmellyClasses/AudioWriter.txt"),
                    "AudioWriter",
                    0
                }
            };
    }
}
