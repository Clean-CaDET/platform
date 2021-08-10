using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    public interface IDataSetAnalysisService
    {
        public Result<string> FindInstancesWithAllDisagreeingAnnotations(IDictionary<string, string> projects);
        public Result<string> FindInstancesRequiringAdditionalAnnotation(IDictionary<string, string> projects);
        public Result<List<DataSetInstance>> FindInstancesWithAllDisagreeingAnnotations(int dataSetId);
        public Result<List<DataSetInstance>> FindInstancesRequiringAdditionalAnnotation(int dataSetId);
    }
}
