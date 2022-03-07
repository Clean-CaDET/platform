using AutoMapper;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSets.Repository;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;

namespace DataSetExplorer.Core.Annotations
{
    public class DataSetAnnotationService : IDataSetAnnotationService
    {
        private readonly IDataSetInstanceRepository _dataSetInstanceRepository;
        private readonly IDataSetAnnotationRepository _dataSetAnnotationRepository;

        public DataSetAnnotationService(IMapper mapper, IDataSetInstanceRepository dataSetInstanceRepository, IDataSetAnnotationRepository dataSetAnnotationRepository)
        {
            _dataSetInstanceRepository = dataSetInstanceRepository;
            _dataSetAnnotationRepository = dataSetAnnotationRepository;
        }

        public Result<Annotation> AddDataSetAnnotation(Annotation annotation, int dataSetInstanceId, int annotatorId)
        {
            var instance = _dataSetInstanceRepository.GetDataSetInstance(dataSetInstanceId);
            if (instance == default) return Result.Fail<Annotation>($"DataSetInstance with id: {dataSetInstanceId} does not exist.");
            var annotator = _dataSetAnnotationRepository.GetAnnotator(annotatorId);
            if (annotator == default) return Result.Fail<Annotation>($"Annotator with id: {annotatorId} does not exist.");
            var codeSmell = _dataSetAnnotationRepository.GetCodeSmell(annotation.InstanceSmell.Name);
            if (codeSmell == default) return Result.Fail<Annotation>($"CodeSmell with value: {annotation.InstanceSmell.Name} does not exist.");
            annotation = new Annotation(codeSmell, annotation.Severity, annotator, annotation.ApplicableHeuristics, annotation.Note);
            instance.AddAnnotation(annotation);
            _dataSetInstanceRepository.Update(instance);
            return Result.Ok(annotation);
        }

        public Result<Annotation> UpdateAnnotation(Annotation changed, int annotationId, int annotatorId)
        {
            var annotation = _dataSetAnnotationRepository.GetDataSetAnnotation(annotationId);
            if (annotation == default) return Result.Fail<Annotation>($"DataSetAnnotation with id: {annotationId} does not exist.");
            if (annotation.Annotator.Id != annotatorId) return Result.Fail<Annotation>($"Only creator can update annotation.");
            var codeSmell = _dataSetAnnotationRepository.GetCodeSmell(changed.InstanceSmell.Name);
            if (codeSmell == default) return Result.Fail<Annotation>($"CodeSmell with value: {changed.InstanceSmell.Name} does not exist.");
            annotation.Update(new Annotation(codeSmell, changed.Severity, annotation.Annotator, changed.ApplicableHeuristics, changed.Note));
            _dataSetAnnotationRepository.Update(annotation);
            return Result.Ok(annotation);
        }

        public Result<InstanceDTO> GetInstanceWithRelatedInstances(int id)
        {
            var instance = _dataSetInstanceRepository.GetInstanceWithRelatedInstances(id);
            if (instance == default) return Result.Fail($"Instance with id: {id} does not exist.");
            return Result.Ok(instance);
        }

        public Result<Instance> GetInstanceWithAnnotations(int id)
        {
            var instance = _dataSetInstanceRepository.GetInstanceWithAnnotations(id);
            if (instance == default) return Result.Fail($"Instance with id: {id} does not exist.");
            return Result.Ok(instance);
        }
    }
}
