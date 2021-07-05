using FluentResults;

namespace DataSetExplorer
{
    public interface IDataSetExporterService
    {
        public Result<string> Export(string outputPath);
    }
}
