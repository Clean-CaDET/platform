using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    public interface IDataSetAnalysisService
    {
        Result<string> FindInstancesWithAllDisagreeingAnnotations(IDictionary<string, string> projects);
        Result<string> FindInstancesRequiringAdditionalAnnotation(IDictionary<string, string> projects);
        Result<List<DataSetInstance>> FindInstancesWithAllDisagreeingAnnotations(int dataSetId);
        Result<List<DataSetInstance>> FindInstancesRequiringAdditionalAnnotation(int dataSetId);
    }
}
