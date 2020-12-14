using System.IO;
using System.Text;
using DataSetExplorer.DataSetBuilder.Model;

namespace DataSetExplorer.DataSetBuilder
{
    class TextFileExporter
    {
        private readonly DataSet _dataSet;
        private readonly string _resultFolder;
        private readonly string _classFileName = "classNames.txt";
        private readonly string _functionFileName = "functionNames.txt";

        public TextFileExporter(string destinationPath, DataSet dataSet)
        {
            _dataSet = dataSet;
            _resultFolder = destinationPath;
        }

        public void ExtractNamesToFile()
        {
            SaveInstancesToFile(SnippetType.Class, _classFileName);
            SaveInstancesToFile(SnippetType.Function, _functionFileName);
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