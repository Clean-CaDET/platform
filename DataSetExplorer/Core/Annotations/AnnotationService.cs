using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Core.DataSets;
using DataSetExplorer.Core.DataSets.Repository;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.Core.Annotations
{
    public class AnnotationService : IAnnotationService
    {
        private readonly IInstanceRepository _instanceRepository;
        private readonly IAnnotationRepository _annotationRepository;
        private readonly IInstanceService _instanceService;

        public AnnotationService(IInstanceRepository instanceRepository, IAnnotationRepository annotationRepository,
            IInstanceService instanceService)
        {
            _instanceRepository = instanceRepository;
            _annotationRepository = annotationRepository;
            _instanceService = instanceService;
        }

        public Result<Annotation> AddAnnotation(Annotation annotation, int instanceId, int annotatorId)
        {
            var instance = _instanceRepository.Get(instanceId);
            if (instance == default) return Result.Fail<Annotation>($"DataSetInstance with id: {instanceId} does not exist.");
            var annotator = _annotationRepository.GetAnnotator(annotatorId);
            if (annotator == default) return Result.Fail<Annotation>($"Annotator with id: {annotatorId} does not exist.");
            var codeSmell = _annotationRepository.GetCodeSmell(annotation.InstanceSmell.Name);
            if (codeSmell == default) return Result.Fail<Annotation>($"CodeSmell with value: {annotation.InstanceSmell.Name} does not exist.");
            annotation = new Annotation(codeSmell, annotation.Severity, annotator, annotation.ApplicableHeuristics, annotation.Note);
            instance.AddAnnotation(annotation);
            _instanceRepository.Update(instance);
            return Result.Ok(annotation);
        }

        public Result<Annotation> UpdateAnnotation(Annotation changed, int annotationId, int annotatorId)
        {
            var annotation = _annotationRepository.Get(annotationId);
            if (annotation == default) return Result.Fail<Annotation>($"DataSetAnnotation with id: {annotationId} does not exist.");
            if (annotation.Annotator.Id != annotatorId) return Result.Fail<Annotation>($"Only creator can update annotation.");
            var codeSmell = _annotationRepository.GetCodeSmell(changed.InstanceSmell.Name);
            if (codeSmell == default) return Result.Fail<Annotation>($"CodeSmell with value: {changed.InstanceSmell.Name} does not exist.");
            annotation.Update(new Annotation(codeSmell, changed.Severity, annotation.Annotator, changed.ApplicableHeuristics, changed.Note));
            _annotationRepository.Update(annotation);
            return Result.Ok(annotation);
        }

        public Result UpdateAnnotationsAfterHeuristicDeletion(CodeSmellDefinition codeSmellDefinition, HeuristicDefinition heuristic)
        {
            foreach (var instance in _instanceService.GetInstancesForSmell(codeSmellDefinition.Name).Value)
            {
                RemoveDeletedHeuristicFromAnnotations(instance.Annotations, heuristic);
            }
            return Result.Ok();
        }

        private void RemoveDeletedHeuristicFromAnnotations(ISet<Annotation> annotations, HeuristicDefinition heuristic)
        {
            foreach (var annotation in annotations)
            {
                var appliedHeuristic = annotation.ApplicableHeuristics.Find(h => h.Description.Equals(heuristic.Name));
                if (appliedHeuristic == null) continue;
                _annotationRepository.DeleteHeuristic(appliedHeuristic.Id);
            }
        }

        public Result UpdateAnnotationsAfterSeverityDeletion(CodeSmellDefinition codeSmellDefinition, SeverityDefinition severity)
        {
            foreach (var instance in _instanceService.GetInstancesForSmell(codeSmellDefinition.Name).Value)
            {
                DeleteAnnotationsWithDeletedSeverity(instance.Annotations, severity);
            }
            return Result.Ok();
        }

        private void DeleteAnnotationsWithDeletedSeverity(ISet<Annotation> annotations, SeverityDefinition severity)
        {
            foreach (var annotation in annotations)
            {
                if (annotation.Severity.Equals(severity.Value)) _annotationRepository.Delete(annotation.Id);
            }
        }

        public Result UpdateAnnotationsAfterSeverityUpdate(CodeSmellDefinition codeSmellDefinition, SeverityDefinition severity, SeverityDefinition oldSeverity)
        {
            foreach (var instance in _instanceService.GetInstancesForSmell(codeSmellDefinition.Name).Value)
            {
                if (instance.Annotations.Count > 0) UpdateAnnotationsWithUpdatedSeverity(instance.Annotations, severity, oldSeverity);
            }
            return Result.Ok();
        }

        private void UpdateAnnotationsWithUpdatedSeverity(ISet<Annotation> annotations, SeverityDefinition severity, SeverityDefinition oldSeverity)
        {
            foreach (var annotation in annotations)
            {
                if (annotation.Severity.Equals(oldSeverity.Value))
                {
                    annotation.Severity = severity.Value;
                    _annotationRepository.Update(annotation);
                }
            }
        }

        public Result UpdateAnnotationsAfterHeuristicUpdate(CodeSmellDefinition codeSmellDefinition, HeuristicDefinition heuristic, HeuristicDefinition oldHeuristic)
        {
            foreach (var instance in _instanceService.GetInstancesForSmell(codeSmellDefinition.Name).Value)
            {
                UpdateAnnotationsWithUpdatedHeuristic(instance.Annotations, heuristic, oldHeuristic);
            }
            return Result.Ok();
        }

        private void UpdateAnnotationsWithUpdatedHeuristic(ISet<Annotation> annotations, HeuristicDefinition heuristic, HeuristicDefinition oldHeuristic)
        {
            foreach (var annotation in annotations)
            {
                var appliedHeuristic = annotation.ApplicableHeuristics.Find(h => h.Description.Equals(oldHeuristic.Name));
                if (appliedHeuristic == null) continue;
                appliedHeuristic.Description = heuristic.Name;
                _annotationRepository.UpdateAppliedHeuristic(appliedHeuristic);
            }
        }

        public Result<List<CodeSmell>> GetCodeSmellsByDefinition(CodeSmellDefinition codeSmellDefinition)
        {
            return Result.Ok(_annotationRepository.GetCodeSmellsByDefinition(codeSmellDefinition));
        }

        public Result<CodeSmell> UpdateCodeSmell(CodeSmell codeSmell)
        {
            return Result.Ok(_annotationRepository.UpdateCodeSmell(codeSmell));
        }
    }
}
