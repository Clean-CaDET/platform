using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    public interface IDataSetExportationService
    {
        public Result<string> Export(IDictionary<string, string> projects, List<Annotator> annotators, string outputPath);
    }
}
