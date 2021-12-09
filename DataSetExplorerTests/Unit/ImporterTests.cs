using DataSetExplorer.DataSets.Model;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorer.Tests.DataFactories;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DataSetExplorer.Tests.Unit
{
    public class ImporterTests
    {
        [Fact]
        public void Imports_data_set_instances_and_annotations()
        {
            var codeSmells = new List<CodeSmell>
            {
                new CodeSmell("Long_Method"),
                new CodeSmell("Large_Class")
            };
            var dataSet = new DataSet("Test", codeSmells);
            ExcelImporter importer = new ExcelImporter(new ExcelFactory().GetTestDataFolder());
            var project = importer.Import("BurningKnight");
            dataSet.AddProject(project);

            var longMethodCandidate = dataSet.Projects.First(p => p.Name.Equals("BurningKnight")).CandidateInstances.First(c => c.CodeSmell.Name.Equals("Long_Method"));
            var distinctMethods = longMethodCandidate.Instances.Distinct();
            distinctMethods.Count().ShouldBe(311);
            distinctMethods.Count(i => i.Annotations.Count == 3).ShouldBe(298);
            distinctMethods.Count(i => i.Annotations.Count == 2).ShouldBe(8);
            distinctMethods.Count(i => i.Annotations.Count == 1).ShouldBe(5);

            var largeClassCandidate = dataSet.Projects.First(p => p.Name.Equals("BurningKnight")).CandidateInstances.First(c => c.CodeSmell.Name.Equals("Large_Class"));
            var distinctClasses = largeClassCandidate.Instances.Distinct();
            distinctClasses.Count().ShouldBe(79);
            distinctClasses.Count(i => i.Annotations.Count == 3).ShouldBe(79);

            var annotators = new List<Annotator>()
            {
                new Annotator(1, 6, 1),
                new Annotator(2, 2, 2),
                new Annotator(3, 2, 3)
            };
            JoinInstancesAndAnnotators(distinctClasses.ToList(), annotators);

            var allClassAnnotations = distinctClasses.SelectMany(c => c.Annotations);
            allClassAnnotations.First(a => a.Annotator.Id == 1).Annotator.ShouldBe(annotators.Find(a => a.Id == 1));
            allClassAnnotations.First(a => a.Annotator.Id == 2).Annotator.ShouldBe(annotators.Find(a => a.Id == 2));
            allClassAnnotations.First(a => a.Annotator.Id == 3).Annotator.ShouldBe(annotators.Find(a => a.Id == 3));
        }

        private void JoinInstancesAndAnnotators(List<Instance> annotatedInstances, List<Annotator> annotators)
        {
            foreach (var annotation in annotatedInstances.SelectMany(i => i.Annotations))
            {
                annotation.Annotator = annotators.Find(annotator => annotator.Id.Equals(annotation.Annotator.Id));
            }
        }

        [Fact]
        public void Finds_insufficiently_annotated_instances()
        {
            ExcelImporter importer = new ExcelImporter(new ExcelFactory().GetTestDataFolder());
            var project = importer.Import("BurningKnight");

            var instances = project.GetInsufficientlyAnnotatedInstances();
            var classes = instances.Find(c => c.CodeSmell.Name.Equals("Large_Class")).Instances;
            var methods = instances.Find(c => c.CodeSmell.Name.Equals("Long_Method")).Instances;
            classes.Count.ShouldBe(0);
            methods.Count.ShouldBe(7);
        }

        [Fact]
        public void Finds_instances_with_all_disagreeing_annotations()
        {
            ExcelImporter importer = new ExcelImporter(new ExcelFactory().GetTestDataFolder());
            var project = importer.Import("BurningKnight");

            var instances = project.GetInstancesWithAllDisagreeingAnnotations();
            var classes = instances.Find(c => c.CodeSmell.Name.Equals("Large_Class")).Instances;
            var methods = instances.Find(c => c.CodeSmell.Name.Equals("Long_Method")).Instances;
            classes.Count.ShouldBe(4);
            methods.Count.ShouldBe(10);
        }
    }
}
