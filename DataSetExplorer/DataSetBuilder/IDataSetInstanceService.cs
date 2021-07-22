using DataSetExplorer.Controllers.DataSetInstance.DTOs;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder
{
    public interface IDataSetInstanceService
    {
        public Result<string> AddDataSetAnnotation(DataSetAnnotationDTO annotation, int annotatorId);
    }
}
