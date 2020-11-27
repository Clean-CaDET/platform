using System;
using PlatformInteractionTool.DataSet;
using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace PlatformInteractionTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new DataSetBuilder("C:\\sdataset3\\", LanguageEnum.CSharp, true, true);

            var project = builder.IncludeMembersWith(5).RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();

            var fileSerializer = new DataSetFileSerializer("C:\\DSOutput\\", project);
            fileSerializer.ExtractNamesToFile();
        }
    }
}
