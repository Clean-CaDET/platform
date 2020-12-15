using System.IO;
using System.Text;
using DataSetExplorer.DataSetBuilder.Model;

namespace DataSetExplorer.DataSetBuilder
{
    class TextFileExporter
    {
        private readonly DataSet _dataSet;
        private readonly string _resultFolder;
        private const string ClassFileName = "classNames.txt";
        private const string FunctionFileName = "functionNames.txt";
        private const string ClassLinks = "classLinks.txt";
        private const string FunctionLinks = "functionLinks.txt";

        public TextFileExporter(string destinationPath, DataSet dataSet)
        {
            _dataSet = dataSet;
            _resultFolder = destinationPath;
        }

        public void ExtractNamesToFile()
        {
            SaveSnippetIdToFile(SnippetType.Class, ClassFileName);
            SaveSnippetIdToFile(SnippetType.Function, FunctionFileName);
            SaveSnippetLinkToFile(SnippetType.Class, ClassLinks);
            SaveSnippetLinkToFile(SnippetType.Function, FunctionLinks);
        }

        private void SaveSnippetIdToFile(SnippetType codeSnippetType, string fileName)
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

        private void SaveSnippetLinkToFile(SnippetType codeSnippetType, string fileName)
        {
            var sb = new StringBuilder();
            foreach (var instance in _dataSet.GetInstancesOfType(codeSnippetType))
            {
                sb.Append(instance.Link).Append("\n");
            }
            WriteToFile(sb.ToString(), fileName);
        }
    }
}