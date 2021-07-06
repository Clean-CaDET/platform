using FluentResults;

namespace DataSetExplorer
{
    public interface IDataSetExportationService
    {
        public Result<string> Export(string outputPath);
    }
}
