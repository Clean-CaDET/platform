using FluentResults;
using System.Collections.Specialized;

namespace DataSetExplorer
{
    interface IDataSetAnalysisService
    {
        public Result<string> FindInstancesWithAllDisagreeingAnnotations(ListDictionary projects);
        public Result<string> FindInstancesRequiringAdditionalAnnotation(ListDictionary projects);
    }
}
