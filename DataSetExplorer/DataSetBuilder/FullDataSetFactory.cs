using CodeModel;
using CodeModel.CaDETModel;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace DataSetExplorer
{
    class FullDataSetFactory
    {

        public IEnumerable<IGrouping<string, DataSetInstance>> GetAnnotatedInstancesGroupedBySmells(ListDictionary projects, List<Annotator> annotators, int? annotatorId)
        {
            var allAnnotatedInstances = new List<DataSetInstance>();
            foreach (var key in projects.Keys)
            {
                CodeModelFactory factory = new CodeModelFactory();
                CaDETProject project = factory.CreateProjectWithCodeFileLinks(key.ToString());

                var importer = new ExcelImporter(projects[key].ToString());
                var annotatedInstances = importer.Import(key.ToString())._instances.ToList();

                LoadAnnotators(annotators, annotatedInstances);
                if (annotatorId != null) annotatedInstances = annotatedInstances.Where(i => i.IsAnnotatedBy((int)annotatorId)).ToList();
                allAnnotatedInstances.AddRange(FillInstancesWithMetrics(annotatedInstances, project));
            }
            return allAnnotatedInstances.GroupBy(i => i.Annotations.ToList()[0].InstanceSmell.Value);
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
