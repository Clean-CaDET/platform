using PlatformInteractionTool.ModelInformationExtractors;

namespace PlatformInteractionTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var informationExtractor = new CaDETProjectExtractor("C:\\sdataset\\");
            informationExtractor.ExtractNamesToFile();
        }
    }
}
