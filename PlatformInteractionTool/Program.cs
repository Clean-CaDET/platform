using PlatformInteractionTool.DataSetBuilder;
using RepositoryCompiler.CodeModel;

namespace PlatformInteractionTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new CaDETToDataSetBuilder("C:\\sdataset4\\", LanguageEnum.CSharp, true, true);

            var project = builder.IncludeMembersWith(2).RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();

            var fileSerializer = new TextFileExporter("C:\\DSOutput\\", project);
            fileSerializer.ExtractNamesToFile();
        }
    }
}
