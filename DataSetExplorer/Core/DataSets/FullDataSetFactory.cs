﻿using System.Collections.Generic;
using System.Linq;
using CodeModel;
using CodeModel.CaDETModel;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSets.Repository;
using DataSetExplorer.Core.DataSetSerializer;

namespace DataSetExplorer.Core.DataSets
{
    public class FullDataSetFactory
    {
        private readonly IInstanceRepository _instanceRepository;
        private readonly IAnnotationRepository _annotationRepository;
        private readonly IProjectService _projectService;

        public FullDataSetFactory(IInstanceRepository instanceRepository, IAnnotationRepository annotationRepository,
            IProjectService projectService)
        {
            _instanceRepository = instanceRepository;
            _annotationRepository = annotationRepository;
            _projectService = projectService;
        }

        public FullDataSetFactory() { }

        public List<SmellCandidateInstances> GetAnnotatedInstancesGroupedBySmells(IDictionary<string, string> projects, List<Annotator> annotators, int? annotatorId)
        {
            var allAnnotatedInstances = new List<SmellCandidateInstances>();
            foreach (var projectSourceLocation in projects.Keys)
            {
                CodeModelFactory factory = new CodeModelFactory();
                CaDETProject project = factory.CreateProjectWithCodeFileLinks(projectSourceLocation);

                var importer = new ExcelImporter(projects[projectSourceLocation], _annotationRepository);
                var annotatedCandidates = importer.Import(projectSourceLocation).CandidateInstances.ToList();

                LoadAnnotators(annotators, annotatedCandidates);
                if (annotatorId != null) GetAnnotatorsInstances(annotatedCandidates, (int)annotatorId);
                AddInstancesToCandidates(allAnnotatedInstances, FillInstancesWithMetrics(annotatedCandidates, project));
            }
            return allAnnotatedInstances;
        }

        public List<SmellCandidateInstances> GetAnnotatedInstancesGroupedBySmells(int datasetId, List<Annotator> annotators, string[] annotationsFilesPaths)
        {
            var allAnnotatedInstances = new List<SmellCandidateInstances>();
            var projects = _projectService.GetAllByDatasetId(datasetId).Value;

            foreach (var project in projects)
            {
                AddInstancesToCandidates(allAnnotatedInstances, project.CandidateInstances.ToList());
                var importer = new ExcelImporter(_annotationRepository);
                foreach (var annotationsFilePath in annotationsFilesPaths)
                {
                    if (project.Url.Equals(importer.GetProjectUrl(annotationsFilePath)))
                    {
                        var importedProject = importer.ImportAnnotationsFile(annotationsFilePath);
                        if (importedProject.Url.Equals(project.Url)) JoinAnnotations(project, importedProject);
                    }
                }
            }
            return allAnnotatedInstances;
        }

        private void JoinAnnotations(DataSetProject project, DataSetProject importedProject)
        {
            foreach(var candidate in project.CandidateInstances)
            {
                var importedCandidate = importedProject.CandidateInstances.First(c => c.CodeSmell.Name.Equals(candidate.CodeSmell.Name));
                foreach (var importedInstance in importedCandidate.Instances)
                {
                    var instance = candidate.Instances.Find(x => x.Link.Equals(importedInstance.Link));
                    importedInstance.Annotations.ToList().ForEach(a => instance.AddAnnotation(a));
                }
            }
        }

        private static void AddInstancesToCandidates(List<SmellCandidateInstances> allAnnotatedInstances, List<SmellCandidateInstances> annotatedInstances)
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

        private static void GetAnnotatorsInstances(List<SmellCandidateInstances> annotatedCandidates, int annotatorId)
        {
            for (var i = 0; i < annotatedCandidates.Count(); i++)
            {
                var instancesByAnnotator = annotatedCandidates[i].Instances.Where(i => i.IsAnnotatedBy(annotatorId)).ToList();
                annotatedCandidates[i] = new SmellCandidateInstances(annotatedCandidates[i].CodeSmell, instancesByAnnotator);
            }
        }

        public List<SmellCandidateInstances> GetAnnotatedInstancesGroupedBySmells(int projectId, int? annotatorId)
        {
            var candidateInstances = new List<SmellCandidateInstances>();
            var instances = new List<Instance>();
            if (annotatorId != null) instances = _instanceRepository.GetInstancesAnnotatedByAnnotator(projectId, annotatorId).ToList();
            else instances = _instanceRepository.GetAnnotatedInstances(projectId).ToList();

            var instancesBySmell = instances.GroupBy(i => i.Annotations.ToList()[0].InstanceSmell.Name);
            foreach (var group in instancesBySmell)
            {
                if (_annotationRepository != null) candidateInstances.Add(new SmellCandidateInstances(_annotationRepository.GetCodeSmell(group.Key), group.ToList()));
                else candidateInstances.Add(new SmellCandidateInstances(new CodeSmell(group.Key), group.ToList()));
            }
            return candidateInstances;
        }

        private static List<SmellCandidateInstances> FillInstancesWithMetrics(List<SmellCandidateInstances> annotatedCandidates, CaDETProject project)
        {
            for (var i = 0; i < annotatedCandidates.Count(); i++)
            {
                var instances = annotatedCandidates[i].Instances.Select(i =>
                {
                    i.MetricFeatures = project.GetMetricsForCodeSnippet(i.CodeSnippetId);
                    return i;
                }).ToList();
                annotatedCandidates[i] = new SmellCandidateInstances(annotatedCandidates[i].CodeSmell, instances);
            }
            return annotatedCandidates;
        }

        private static void LoadAnnotators(List<Annotator> annotators, List<SmellCandidateInstances> annotatedCandidates)
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
