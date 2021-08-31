using CodeModel;
using CodeModel.CaDETModel;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer
{
    class FullDataSetFactory
    {
        public IEnumerable<IGrouping<string, DataSetInstance>> GetAnnotatedInstancesGroupedBySmells(IDictionary<string, string> projects, List<Annotator> annotators, int? annotatorId)
        {
            var allAnnotatedInstances = new List<DataSetInstance>();
            foreach (var projectSourceLocation in projects.Keys)
            {
                CodeModelFactory factory = new CodeModelFactory();
                CaDETProject project = factory.CreateProjectWithCodeFileLinks(projectSourceLocation);

                var importer = new ExcelImporter(projects[projectSourceLocation]);
                var annotatedInstances = importer.Import(projectSourceLocation).Instances.ToList();

                LoadAnnotators(annotators, annotatedInstances);
                if (annotatorId != null) annotatedInstances = annotatedInstances.Where(i => i.IsAnnotatedBy((int)annotatorId)).ToList();
                allAnnotatedInstances.AddRange(FillInstancesWithMetrics(annotatedInstances, project));
            }
            return allAnnotatedInstances.GroupBy(i => i.Annotations.ToList()[0].InstanceSmell.Name);
        }

        private List<DataSetInstance> FillInstancesWithMetrics(List<DataSetInstance> annotatedInstances, CaDETProject project)
        {
            return annotatedInstances.Select(i => {
                i.MetricFeatures = project.GetMetricsForCodeSnippet(i.CodeSnippetId);
                return i;
            }).ToList();
        }

        private void LoadAnnotators(List<Annotator> annotators, List<DataSetInstance> annotatedInstances)
        {
            foreach (var annotation in annotatedInstances.SelectMany(i => i.Annotations))
            {
                annotation.Annotator = annotators.Find(annotator => annotator.Id.Equals(annotation.Annotator.Id));
            }
        }
    }
}
