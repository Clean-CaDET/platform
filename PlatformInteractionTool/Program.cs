using System.IO;
using System.Text;
using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace PlatformInteractionTool
{
    class Program
    {
        private static string _basePath = "C:\\student datasets\\";

        static void Main(string[] args)
        {
            ParseStudentProjectsToFiles();
        }

        private static void ParseStudentProjectsToFiles()
        {
            CodeModelBuilder builder = new CodeModelBuilder(LanguageEnum.CSharp);

            for (int i = 1; i <= 38; i++)
            {
                var project = builder.ParseFiles(_basePath + i);
                SaveToFile(i, project);
            }
        }

        private static void SaveToFile(int id, CaDETProject project)
        {
            var sb = new StringBuilder();
            foreach (var projectClass in project.Classes)
            {
                sb.Append(projectClass.FullName).Append("\n");
                sb.Append(projectClass.Metrics.LCOM).Append(",");
                sb.Append(projectClass.Metrics.LOC).Append(",");
                sb.Append(projectClass.Metrics.NAD).Append(",");
                sb.Append(projectClass.Metrics.NMD).Append(",");
                sb.Append(projectClass.Metrics.WMC).Append("\n");
            }
            File.WriteAllText(_basePath + id.ToString() + ".txt", sb.ToString());
        }
    }
}
