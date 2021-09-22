using CodeModel;
using CodeModel.CaDETModel;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetBuilder.Model.Repository;
using DataSetExplorer.DataSetSerializer;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer
{
    public class FullDataSetFactory
    {
        private readonly IDataSetInstanceRepository _instanceRepository;

        public FullDataSetFactory(IDataSetInstanceRepository instanceRepository)
        {
            _instanceRepository = instanceRepository;
        }

        public FullDataSetFactory() { }

        public List<CandidateDataSetInstance> GetAnnotatedInstancesGroupedBySmells(IDictionary<string, string> projects, List<Annotator> annotators, int? annotatorId)
        {
            var allAnnotatedInstances = new List<CandidateDataSetInstance>();
            foreach (var projectSourceLocation in projects.Keys)
            {
                CodeModelFactory factory = new CodeModelFactory();
                CaDETProject project = factory.CreateProjectWithCodeFileLinks(projectSourceLocation);

                var importer = new ExcelImporter(projects[projectSourceLocation]);
                var annotatedCandidates = importer.Import(projectSourceLocation).CandidateInstances.ToList();

                LoadAnnotators(annotators, annotatedCandidates);
                if (annotatorId != null) GetAnnotatorsInstances(annotatedCandidates, (int)annotatorId);
                AddInstancesToCandidates(allAnnotatedInstances, FillInstancesWithMetrics(annotatedCandidates, project));
            }
            return allAnnotatedInstances;
        }

        private static void AddInstancesToCandidates(List<CandidateDataSetInstance> allAnnotatedInstances, List<CandidateDataSetInstance> annotatedInstances)
        {
            foreach (var annotated in annotatedInstances)
            {
                if (allAnnotatedInstances.Exists(c => c.CodeSmell.Name.Equals(annotated.CodeSmell.Name)))
                {
                    var index = allAnnotatedInstances.FindIndex(c => c.CodeSmell.Name.Equals(annotated.CodeSmell.Name));
                    allAnnotatedInstances[index].Instances.AddRange(annotated.Instances);
                }
                else
                {
                    allAnnotatedInstances.Add(annotated);
                }
            }
        }

        private static void GetAnnotatorsInstances(List<CandidateDataSetInstance> annotatedCandidates, int annotatorId)
        {
            for (var i = 0; i < annotatedCandidates.Count(); i++)
            {
                var instancesByAnnotator = annotatedCandidates[i].Instances.Where(i => i.IsAnnotatedBy(annotatorId)).ToList();
                annotatedCandidates[i] = new CandidateDataSetInstance(annotatedCandidates[i].CodeSmell, instancesByAnnotator);
            }
        }

        public List<CandidateDataSetInstance> GetAnnotatedInstancesGroupedBySmells(int projectId, int? annotatorId)
        {
            var candidateInstances = new List<CandidateDataSetInstance>();
            var instances = new List<DataSetInstance>();
            if (annotatorId != null) instances = _instanceRepository.GetInstancesAnnotatedByAnnotator(projectId, annotatorId).ToList();
            else instances = _instanceRepository.GetAnnotatedInstances(projectId).ToList();

            var groupes = instances.GroupBy(i => i.Annotations.ToList()[0].InstanceSmell.Name);
            foreach (var group in groupes)
            {
                candidateInstances.Add(new CandidateDataSetInstance(new CodeSmell(group.Key), group.ToList()));
            }
            return candidateInstances;
        }

        private static List<CandidateDataSetInstance> FillInstancesWithMetrics(List<CandidateDataSetInstance> annotatedCandidates, CaDETProject project)
        {
            for (var i = 0; i < annotatedCandidates.Count(); i++)
            {
                var instances = annotatedCandidates[i].Instances.Select(i =>
                {
                    i.MetricFeatures = project.GetMetricsForCodeSnippet(i.CodeSnippetId);
                    return i;
                }).ToList();
                annotatedCandidates[i] = new CandidateDataSetInstance(annotatedCandidates[i].CodeSmell, instances);
            }
            return annotatedCandidates;
        }

        private static void LoadAnnotators(List<Annotator> annotators, List<CandidateDataSetInstance> annotatedCandidates)
        {
            foreach (var candidate in annotatedCandidates)
            {
                foreach (var annotation in candidate.Instances.SelectMany(i => i.Annotations))
                {
                    annotation.Annotator = annotators.Find(annotator => annotator.Id.Equals(annotation.Annotator.Id));
                }
            }
        }
    }
}
