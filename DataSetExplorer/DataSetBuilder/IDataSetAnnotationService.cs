using DataSetExplorer.Controllers.Annotation.DTOs;
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
        Result<List<CodeSmellDTO>> GetAllCodeSmells();
        Result<DataSetAnnotation> AddDataSetAnnotation(DataSetAnnotation annotation, int dataSetInstanceId, int annotatorId);
        Result<DataSetAnnotation> UpdateAnnotation(DataSetAnnotation changed, int annotationId, int annotatorId);
    }
}
