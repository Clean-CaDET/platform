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
            classes.Count.ShouldBe(79);
            classes.Count(c => c.Annotations.Count == 3).ShouldBe(79);

            var functions = dataSet.GetInstancesOfType(SnippetType.Function);
            functions.Count.ShouldBe(313);
            functions.Count(c => c.Annotations.Count == 1).ShouldBe(7);
            functions.Count(c => c.Annotations.Count == 2).ShouldBe(10);
            functions.Count(c => c.Annotations.Count == 3).ShouldBe(296);
        }

        [Fact]
        public void Finds_insufficiently_annotated_instances()
        {
            ExcelImporter importer = new ExcelImporter(new ExcelFactory().GetTestDataFolder());
            var dataSet = importer.Import("BurningKnight");

            var instances = dataSet.GetInsufficientlyAnnotatedInstances();

            instances.Count.ShouldBe(9);
        }

        [Fact]
        public void Finds_instances_with_all_disagreeing_annotations()
        {
            ExcelImporter importer = new ExcelImporter(new ExcelFactory().GetTestDataFolder());
            var dataSet = importer.Import("BurningKnight");

            var instances = dataSet.GetInstancesWithAllDisagreeingAnnotations();

            instances.Count.ShouldBe(21);
        }
    }
}
