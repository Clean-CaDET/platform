using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder
{
    public interface IDataSetAnnotationService
    {
        Result<string> AddDataSetAnnotation(DataSetAnnotation annotation, int dataSetInstanceId, int annotatorId);
        Result<string> UpdateAnnotation(DataSetAnnotation changed, int annotationId, int annotatorId);
    }
}
