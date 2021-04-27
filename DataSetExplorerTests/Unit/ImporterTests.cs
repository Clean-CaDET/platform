using System;
using System.Collections.Generic;
using System.IO;
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

            var annotators = ImportAnnotators();
            JoinInstancesAndAnnotators(ref classes, annotators);

            var allClassAnnotations = classes.SelectMany(c => c.Annotations);
            allClassAnnotations.First(a => a.Annotator.Id == 1).Annotator.ShouldBe(annotators.Find(a => a.Id == 1));
            allClassAnnotations.First(a => a.Annotator.Id == 2).Annotator.ShouldBe(annotators.Find(a => a.Id == 2));
            allClassAnnotations.First(a => a.Annotator.Id == 3).Annotator.ShouldBe(annotators.Find(a => a.Id == 3));
        }

        private void JoinInstancesAndAnnotators(ref List<DataSetInstance> annotatedInstances, List<Annotator> annotators)
        {
            foreach (var annotation in annotatedInstances.SelectMany(i => i.Annotations))
            {
                annotation.Annotator = annotators.Find(annotator => annotator.Id.Equals(annotation.Annotator.Id));
            }
        }

        private List<Annotator> ImportAnnotators()
        {
            var annotators = new List<Annotator>();
            using (var reader = new StreamReader("../../../DataFactories/AnnotatorsData/annotators.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    annotators.Add(new Annotator(Int16.Parse(values[0]), Int16.Parse(values[1]), Int16.Parse(values[2])));
                }
            }
            return annotators;
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
