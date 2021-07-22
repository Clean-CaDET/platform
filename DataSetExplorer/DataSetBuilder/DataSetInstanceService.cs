using DataSetExplorer.Controllers.DataSetInstance.DTOs;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetBuilder.Model.Exceptions;
using DataSetExplorer.DataSetBuilder.Model.Repository;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder
{
    public class DataSetInstanceService : IDataSetInstanceService
    {
        private readonly IDataSetInstanceRepository _dataSetInstanceRepository;

        public DataSetInstanceService(IDataSetInstanceRepository dataSetInstanceRepository)
        {
            _dataSetInstanceRepository = dataSetInstanceRepository;
        }

        public Result<string> AddDataSetAnnotation(DataSetAnnotationDTO annotation, int annotatorId)
        {
            var instance = _dataSetInstanceRepository.GetDataSetInstance(annotation.DataSetInstanceId);
            if (instance == default) throw new DataSetInstanceWithIdNotFound(annotation.DataSetInstanceId);
            var annotator = _dataSetInstanceRepository.GetAnnotator(annotatorId);
            if (annotator == default) throw new AnnotatorWithIdNotFound(annotatorId);
            List<SmellHeuristic> smellHeuristics = new List<SmellHeuristic>();
            foreach (var heuristic in annotation.ApplicableHeuristics) smellHeuristics.Add(new SmellHeuristic(heuristic.Description, true, heuristic.Reason));
            DataSetAnnotation dataSetAnnotation = new DataSetAnnotation(annotation.CodeSmell, annotation.Severity, annotator, smellHeuristics);
            instance.AddAnnotation(dataSetAnnotation);
            _dataSetInstanceRepository.AddAnnotation(instance);
            return Result.Ok("Annotation added!");
        }
    }
}
