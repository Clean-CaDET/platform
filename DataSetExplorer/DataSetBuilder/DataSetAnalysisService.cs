using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetBuilder.Model.Exceptions;
using DataSetExplorer.DataSetBuilder.Model.Repository;
using DataSetExplorer.DataSetSerializer;
using FluentResults;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataSetExplorer
{
    public class DataSetAnalysisService : IDataSetAnalysisService
    {
        private readonly IDataSetRepository _dataSetRepository;

        public DataSetAnalysisService(IDataSetRepository dataSetRepository)
        {
            _dataSetRepository = dataSetRepository;
        }

        public List<DataSetInstance> FindInstancesWithAllDisagreeingAnnotations(int dataSetId)
        {
            var dataset = LoadDataSet(dataSetId);
            return dataset.GetInstancesWithAllDisagreeingAnnotations();
        }

        public List<DataSetInstance> FindInstancesRequiringAdditionalAnnotation(int dataSetId)
        {
            var dataset = LoadDataSet(dataSetId);
            return dataset.GetInsufficientlyAnnotatedInstances();
        }

        private DataSet LoadDataSet(int dataSetId)
        {
            var dataset = _dataSetRepository.GetDataSet(dataSetId);
            if (dataset == default) throw new DataSetWithIdNotFound(dataSetId);
            return dataset;
        }
    }
}
