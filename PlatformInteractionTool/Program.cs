using PlatformInteractionTool.DataSetBuilder;
using RepositoryCompiler.CodeModel;

namespace PlatformInteractionTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new Builder("C:\\sdataset4\\", LanguageEnum.CSharp, true, true);

            var project = builder.IncludeMembersWith(2).RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();

            var fileSerializer = new DSFileSerializer("C:\\DSOutput\\", project);
            fileSerializer.ExtractNamesToFile();
        }
    }
}
