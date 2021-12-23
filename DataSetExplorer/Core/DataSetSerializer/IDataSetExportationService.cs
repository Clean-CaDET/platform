using System.Collections.Generic;
using DataSetExplorer.Core.Annotations.Model;
using FluentResults;

namespace DataSetExplorer.Core.DataSetSerializer
{
    public interface IDataSetExportationService
    {
        public Result<string> Export(IDictionary<string, string> projects, List<Annotator> annotators, string outputPath);
    }
}
