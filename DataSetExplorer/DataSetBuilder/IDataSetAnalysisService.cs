using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DataSetExplorer
{
    public interface IDataSetAnalysisService
    {
        public Result<string> FindInstancesWithAllDisagreeingAnnotations(ListDictionary projects);
        public Result<string> FindInstancesRequiringAdditionalAnnotation(ListDictionary projects);
        public Result<List<DataSetInstance>> FindInstancesWithAllDisagreeingAnnotations(int dataSetId);
        public Result<List<DataSetInstance>> FindInstancesRequiringAdditionalAnnotation(int dataSetId);
    }
}
