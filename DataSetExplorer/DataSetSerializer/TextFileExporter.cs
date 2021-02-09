using DataSetExplorer.DataSetBuilder.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        public void ExportInstancesWithAnnotatorId(List<DataSetInstance> instances)
        {
            //TODO: Consider moving to DataSet or new entity
            var groupedInstances = instances.GroupBy(i => i.GetSortedAnnotatorIds());
            foreach (var group in groupedInstances)
            {
                SaveInstanceToFile(group.ToList(), group.Key + ".txt");
                SaveSnippetLinkToFile(group.ToList(), group.Key + "-links.txt");
            }
        }

        public void Export(DataSet dataSet)
        {
            SaveInstanceToFile(dataSet.GetInstancesOfType(SnippetType.Class), ClassFileName);
            SaveInstanceToFile(dataSet.GetInstancesOfType(SnippetType.Function), FunctionFileName);
            SaveSnippetLinkToFile(dataSet.GetInstancesOfType(SnippetType.Class), ClassLinks);
            SaveSnippetLinkToFile(dataSet.GetInstancesOfType(SnippetType.Function), FunctionLinks);
        }

        private void SaveInstanceToFile(List<DataSetInstance> instances, string fileName)
        {
            var sb = new StringBuilder();
            foreach (var instance in instances)
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

        private void SaveSnippetLinkToFile(List<DataSetInstance> instances, string fileName)
        {
            var sb = new StringBuilder();
            foreach (var instance in instances)
            {
                sb.Append(instance.Link).Append("\n");
            }
            WriteToFile(sb.ToString(), fileName);
        }
    }
}