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
        public IEnumerable<IGrouping<string, DataSetInstance>> GetAnnotatedInstancesGroupedBySmells(int? annotatorId)
        {
            var allAnnotatedInstances = new List<DataSetInstance>();

            var projects = GetAnnotatedProjects();
            foreach (var key in projects.Keys)
            {
                CodeModelFactory factory = new CodeModelFactory();
                CaDETProject project = factory.CreateProjectWithCodeFileLinks(key.ToString());

                var importer = new ExcelImporter(projects[key].ToString());
                var annotatedInstances = importer.Import("Clean CaDET").GetAllInstances();

                LoadAnnotators(annotatedInstances);
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

        private ListDictionary GetAnnotatedProjects()
        {
            ListDictionary projects = new ListDictionary(); // local repository path, annotations folder path
            projects.Add("D:/ccadet/annotations/repos/BurningKnight", "D:/ccadet/annotations/annotated/BurningKnight");
            projects.Add("D:/ccadet/annotations/repos/Core2D", "D:/ccadet/annotations/annotated/Core2d");
            projects.Add("D:/ccadet/annotations/repos/jellyfin", "D:/ccadet/annotations/annotated/Jellyfin");
            projects.Add("D:/ccadet/annotations/repos/OpenRA", "D:/ccadet/annotations/annotated/OpenRA");
            projects.Add("D:/ccadet/annotations/repos/ShareX", "D:/ccadet/annotations/annotated/ShareX");
            projects.Add("D:/ccadet/annotations/repos/ShopifySharp", "D:/ccadet/annotations/annotated/ShopifySharp");
            projects.Add("D:/ccadet/annotations/repos/MonoGame", "D:/ccadet/annotations/annotated/MonoGame");
            projects.Add("D:/ccadet/annotations/repos/Osu", "D:/ccadet/annotations/annotated/Osu");
            return projects;
        }

        private void LoadAnnotators(List<DataSetInstance> annotatedInstances)
        {
            List<Annotator> annotators = new List<Annotator>()
            {
                new Annotator(1, 6, 1),
                new Annotator(2, 2, 2),
                new Annotator(3, 2, 3)
            };

            foreach (var annotation in annotatedInstances.SelectMany(i => i.Annotations))
            {
                annotation.Annotator = annotators.Find(annotator => annotator.Id.Equals(annotation.Annotator.Id));
            }
        }
    }
}
