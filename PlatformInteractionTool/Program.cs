using PlatformInteractionTool.ModelInformationExtractors;

namespace PlatformInteractionTool
{
    class Program
    {
        static void Main(string[] args)
        {
            //var informationExtractor = new CaDETProjectExtractor("C:\\sdataset\\");
            var informationExtractor = new CaDETProjectExtractor("C:\\student datasets\\24\\");
            informationExtractor.ExtractNamesToFile();
        }
    }
}
