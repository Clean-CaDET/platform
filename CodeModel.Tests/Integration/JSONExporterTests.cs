using CodeModel.Serialization;
using CodeModel.Tests.DataFactories;
using Shouldly;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace CodeModel.Tests.Integration
{
    public class JSONExporterTests
    {
        private static readonly CodeFactory TestDataFactory = new();

        [Theory]
        [MemberData(nameof(TestData))]
        public void Saves_class_cohesion_graph_to_json(IEnumerable<string> sourceCode, string jsonPath)
        {
            var exporter = new ClassCohesionGraphExporter();
            var project = new CodeModelFactory().CreateProject(sourceCode);
            var actualClass = project.Classes.First();

            exporter.ExportJSON(actualClass, jsonPath);

            File.Exists(jsonPath).ShouldBeTrue();
        }

        public static IEnumerable<object[]> TestData => new List<object[]>
        {
            new object[]
            {
                //TODO: Establish convention for tests that rely on file system across project
                TestDataFactory.ReadClassFromFile(
                    "../../../DataFactories/TestClasses/JSONExporter/IntegrationHelpers.txt"),
                "C:\\CCaDET-Tests\\CodeModel\\JSONExporter\\IntegrationHelpers.json"
            },
            new object[]
            {
                TestDataFactory.ReadClassFromFile(
                    "../../../DataFactories/TestClasses/JSONExporter/RegionCaptureForm.txt"),
                "C:\\CCaDET-Tests\\CodeModel\\JSONExporter\\RegionCaptureForm.json"
            },
            new object[]
            {
                TestDataFactory.ReadClassFromFile(
                    "../../../DataFactories/TestClasses/JSONExporter/ScreenRecorder.txt"),
                "C:\\CCaDET-Tests\\CodeModel\\JSONExporter\\ScreenRecorder.json"
            },
            new object[]
            {
                TestDataFactory.ReadClassFromFile(
                    "../../../DataFactories/TestClasses/JSONExporter/BoxDecoratorViewModel.txt"),
                "C:\\CCaDET-Tests\\CodeModel\\JSONExporter\\BoxDecoratorViewModel.json"
            }
        };
    }
}
