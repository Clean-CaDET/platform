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
        private readonly IDataSetInstanceRepository _dataSetInstanceRepository;

        public DataSetAnnotationService(IDataSetInstanceRepository dataSetInstanceRepository)
        {
            _dataSetInstanceRepository = dataSetInstanceRepository;
        }

        public Result<string> AddDataSetAnnotation(DataSetAnnotation annotation, int dataSetInstanceId, int annotatorId)
        {
            var instance = _dataSetInstanceRepository.GetDataSetInstance(dataSetInstanceId);
            if (instance == default) return Result.Fail<string>($"DataSetInstance with id: {dataSetInstanceId} does not exist.");
            var annotator = _dataSetInstanceRepository.GetAnnotator(annotatorId);
            if (annotator == default) return Result.Fail<string>($"Annotator with id: {annotatorId} does not exist.");
            annotation.Annotator = annotator;
            instance.AddAnnotation(annotation);
            _dataSetInstanceRepository.Update(instance);
            return Result.Ok("Annotation added!");
        }
    }
}
