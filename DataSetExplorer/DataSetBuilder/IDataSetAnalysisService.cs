using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    public interface IDataSetAnalysisService
    {
        public List<DataSetInstance> FindInstancesWithAllDisagreeingAnnotations(int dataSetId);
        public List<DataSetInstance> FindInstancesRequiringAdditionalAnnotation(int dataSetId);
    }
}
