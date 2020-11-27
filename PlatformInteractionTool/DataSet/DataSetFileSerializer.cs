using PlatformInteractionTool.DataSet.Model;
using System.IO;
using System.Text;

namespace PlatformInteractionTool.DataSet
{
    class DataSetFileSerializer
    {
        private readonly DataSetProject _project;
        private readonly string _resultFolder;

        public DataSetFileSerializer(string destinationPath, DataSetProject project)
        {
            _project = project;
            _resultFolder = destinationPath + "extraction-results\\";
        }

        public void ExtractNamesToFile()
        {
            SaveClassNamesToFile();
            SaveMemberNamesToFile();
        }

        private void SaveClassNamesToFile()
        {
            var sb = new StringBuilder();
            foreach (var projectClass in _project.Classes)
            {
                sb.Append(projectClass.FullName).Append("\n");
            }
            WriteToFile(sb.ToString(), "classNames.txt");
        }
        private void SaveMemberNamesToFile()
        {
            var sb = new StringBuilder();
            foreach (var function in _project.Functions)
            {
                sb.Append(function.FullSignature).Append("\n");
            }
            WriteToFile(sb.ToString(), "function.txt");
        }

        private void WriteToFile(string text, string fileName)
        {
            if (!Directory.Exists(_resultFolder)) Directory.CreateDirectory(_resultFolder);
            File.WriteAllText(_resultFolder + fileName, text);
        }
    }
}