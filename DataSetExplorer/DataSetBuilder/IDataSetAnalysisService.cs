using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    public interface IDataSetAnalysisService
    {
        public Result<string> FindInstancesWithAllDisagreeingAnnotations(string dataSetPath, string outputPath);
        public Result<string> FindInstancesRequiringAdditionalAnnotation(string dataSetPath, string outputPath);
        public Result<List<DataSetInstance>> FindInstancesWithAllDisagreeingAnnotations(int dataSetId);
        public Result<List<DataSetInstance>> FindInstancesRequiringAdditionalAnnotation(int dataSetId);
    }
}
