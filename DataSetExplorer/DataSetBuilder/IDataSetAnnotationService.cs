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
        Result<Annotation> AddDataSetAnnotation(Annotation annotation, int dataSetInstanceId, int annotatorId);
        Result<Annotation> UpdateAnnotation(Annotation changed, int annotationId, int annotatorId);
    }
}
