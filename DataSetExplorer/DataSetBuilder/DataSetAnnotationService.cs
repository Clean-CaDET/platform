using AutoMapper;
using DataSetExplorer.Controllers.Annotation.DTOs;
using DataSetExplorer.DataSetBuilder.Model;

using DataSetExplorer.DataSetBuilder.Model.Repository;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder
{
    public class DataSetAnnotationService : IDataSetAnnotationService
    {
        private readonly IMapper _mapper;
        private readonly IDataSetInstanceRepository _dataSetInstanceRepository;
        private readonly IDataSetAnnotationRepository _dataSetAnnotationRepository;

        public DataSetAnnotationService(IMapper mapper, IDataSetInstanceRepository dataSetInstanceRepository, IDataSetAnnotationRepository dataSetAnnotationRepository)
        {
            _mapper = mapper;
            _dataSetInstanceRepository = dataSetInstanceRepository;
            _dataSetAnnotationRepository = dataSetAnnotationRepository;
        }

        public Result<List<CodeSmellDTO>> GetAllCodeSmells()
        {
            var codeSmells = new List<CodeSmellDTO>();
            foreach (var codeSmell in _dataSetAnnotationRepository.GetAllCodeSmells()) codeSmells.Add(_mapper.Map<CodeSmellDTO>(codeSmell));
            return Result.Ok(codeSmells);
        }

        public Result<DataSetAnnotation> AddDataSetAnnotation(DataSetAnnotation annotation, int dataSetInstanceId, int annotatorId)
        {
            var instance = _dataSetInstanceRepository.GetDataSetInstance(dataSetInstanceId);
            if (instance == default) return Result.Fail<DataSetAnnotation>($"DataSetInstance with id: {dataSetInstanceId} does not exist.");
            var annotator = _dataSetAnnotationRepository.GetAnnotator(annotatorId);
            if (annotator == default) return Result.Fail<DataSetAnnotation>($"Annotator with id: {annotatorId} does not exist.");
            var codeSmell = _dataSetAnnotationRepository.GetGodeSmell(annotation.InstanceSmell.Value);
            if (codeSmell == default) return Result.Fail<DataSetAnnotation>($"CodeSmell with value: {annotation.InstanceSmell.Value} does not exist.");
            annotation = new DataSetAnnotation(codeSmell, annotation.Severity, annotator, annotation.ApplicableHeuristics);
            instance.AddAnnotation(annotation);
            _dataSetInstanceRepository.Update(instance);
            return Result.Ok(annotation);
        }

        public Result<DataSetAnnotation> UpdateAnnotation(DataSetAnnotation changed, int annotationId, int annotatorId)
        {
            var annotation = _dataSetAnnotationRepository.GetDataSetAnnotation(annotationId);
            if (annotation == default) return Result.Fail<DataSetAnnotation>($"DataSetAnnotation with id: {annotationId} does not exist.");
            if (annotation.Annotator.Id != annotatorId) return Result.Fail<DataSetAnnotation>($"Only creator can update annotation.");
            var codeSmell = _dataSetAnnotationRepository.GetGodeSmell(changed.InstanceSmell.Value);
            if (codeSmell == default) return Result.Fail<DataSetAnnotation>($"CodeSmell with value: {changed.InstanceSmell.Value} does not exist.");
            annotation.Update(new DataSetAnnotation(codeSmell, changed.Severity, annotation.Annotator, changed.ApplicableHeuristics));
            _dataSetAnnotationRepository.Update(annotation);
            return Result.Ok(annotation);
        }
    }
}
