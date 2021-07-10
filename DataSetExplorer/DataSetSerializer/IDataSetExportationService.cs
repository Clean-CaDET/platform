using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DataSetExplorer
{
    public interface IDataSetExportationService
    {
        public Result<string> Export(ListDictionary projects, List<Annotator> annotators, string outputPath);
    }
}
