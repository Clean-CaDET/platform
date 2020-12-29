using System.Linq;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorerTests.DataFactories;
using Shouldly;
using Xunit;

namespace DataSetExplorerTests.Unit
{
    public class ImporterTests
    {
        [Fact]
        public void Imports_data_set_instances_and_annotations()
        {
            ExcelImporter importer = new ExcelImporter(new ExcelFactory().GetTestDataFolder());

            var dataSet = importer.Import("BurningKnight");

            var classes = dataSet.GetInstancesOfType(SnippetType.Class);
            classes.Count.ShouldBe(78);
            classes.Count(c => c.Annotations.Count == 3).ShouldBe(78);

            var functions = dataSet.GetInstancesOfType(SnippetType.Function);
            functions.Count.ShouldBe(312);
            functions.Count(c => c.Annotations.Count == 1).ShouldBe(7);
            functions.Count(c => c.Annotations.Count == 2).ShouldBe(10);
            functions.Count(c => c.Annotations.Count == 3).ShouldBe(295);
        }

        [Fact]
        public void Finds_instances_for_cross_validation()
        {
            ExcelImporter importer = new ExcelImporter(new ExcelFactory().GetTestDataFolder());
            var dataSet = importer.Import("BurningKnight");

            var instances = dataSet.GetInstancesForCrossValidation();

            instances.Count.ShouldBe(9);
        }
    }
}
