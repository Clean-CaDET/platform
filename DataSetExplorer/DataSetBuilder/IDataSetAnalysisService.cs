using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    public interface IDataSetAnalysisService
    {
        Result<string> FindInstancesWithAllDisagreeingAnnotations(IDictionary<string, string> projects);
        Result<string> FindInstancesRequiringAdditionalAnnotation(IDictionary<string, string> projects);
        Result<List<CandidateDataSetInstance>> FindInstancesWithAllDisagreeingAnnotations(IEnumerable<int> projectIds);
        Result<List<CandidateDataSetInstance>> FindInstancesRequiringAdditionalAnnotation(IEnumerable<int> projectIds);
    }
}
