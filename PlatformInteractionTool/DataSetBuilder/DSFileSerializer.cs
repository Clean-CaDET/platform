using PlatformInteractionTool.DataSetBuilder.Model;
using System.IO;
using System.Text;

namespace PlatformInteractionTool.DataSetBuilder
{
    class DSFileSerializer
    {
        private readonly DataSet _dataSet;
        private readonly string _resultFolder;

        public DSFileSerializer(string destinationPath, DataSet dataSet)
        {
            _dataSet = dataSet;
            _resultFolder = destinationPath + "extraction-results\\";
        }

        public void ExtractNamesToFile()
        {
            SaveInstancesToFile(SnippetType.Class, "classNamesA.txt");
            SaveInstancesToFile(SnippetType.Function, "memberNamesA.txt");
        }

        private void SaveInstancesToFile(SnippetType codeSnippetType, string fileName)
        {
            var sb = new StringBuilder();
            foreach (var instance in _dataSet.GetInstancesOfType(codeSnippetType))
            {
                sb.Append(instance.CodeSnippetId).Append("\n");
            }
            WriteToFile(sb.ToString(), fileName);
        }

        private void WriteToFile(string text, string fileName)
        {
            if (!Directory.Exists(_resultFolder)) Directory.CreateDirectory(_resultFolder);
            File.WriteAllText(_resultFolder + fileName, text);
        }
    }
}