using System.IO;
using System.Text;
using DataSetExplorer.DataSetBuilder.Model;

namespace DataSetExplorer.DataSetSerializer
{
    class TextFileExporter
    {
        private readonly string _resultFolder;
        private const string ClassFileName = "classNames.txt";
        private const string FunctionFileName = "functionNames.txt";
        private const string ClassLinks = "classLinks.txt";
        private const string FunctionLinks = "functionLinks.txt";

        public TextFileExporter(string destinationPath)
        {
            _resultFolder = destinationPath;
        }

        public void Export(DataSet dataSet)
        {
            SaveSnippetIdToFile(SnippetType.Class, ClassFileName, dataSet);
            SaveSnippetIdToFile(SnippetType.Function, FunctionFileName, dataSet);
            SaveSnippetLinkToFile(SnippetType.Class, ClassLinks, dataSet);
            SaveSnippetLinkToFile(SnippetType.Function, FunctionLinks, dataSet);
        }

        private void SaveSnippetIdToFile(SnippetType codeSnippetType, string fileName, DataSet dataSet)
        {
            var sb = new StringBuilder();
            foreach (var instance in dataSet.GetInstancesOfType(codeSnippetType))
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

        private void SaveSnippetLinkToFile(SnippetType codeSnippetType, string fileName, DataSet dataSet)
        {
            var sb = new StringBuilder();
            foreach (var instance in dataSet.GetInstancesOfType(codeSnippetType))
            {
                sb.Append(instance.Link).Append("\n");
            }
            WriteToFile(sb.ToString(), fileName);
        }
    }
}