using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSetBuilder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataSetExplorer.DataSetSerializer
{
    class TextFileExporter
    {
        private readonly string _resultFolder;

        public TextFileExporter(string destinationPath)
        {
            _resultFolder = destinationPath;
        }

        public void ExportInstancesWithAnnotatorId(List<CandidateDataSetInstance> candidateInstances)
        {
            //TODO: Consider moving to DataSet or new entity
            foreach (var candidate in candidateInstances)
            {
                var groupedInstances = candidate.Instances.GroupBy(i => i.GetSortedAnnotatorIds());
                foreach (var group in groupedInstances)
                {
                    SaveInstanceToFile(group.ToList(), candidate.CodeSmell.Name + "_" + group.Key + ".txt");
                    SaveSnippetLinkToFile(group.ToList(), candidate.CodeSmell.Name + "_" + group.Key + "-links.txt");
                }
            }
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

        internal void ExportMembersFromAnnotatedClasses(Dictionary<int, List<CaDETClass>> classesGroupedBySeverity, List<DataSetInstance> annotatedClasses)
        {
            foreach (var severity in classesGroupedBySeverity.Keys)
            {
                Directory.CreateDirectory(_resultFolder + severity + "/");
                for (var i = 0; i < annotatedClasses.Count; i++)
                {
                    var classForExport = classesGroupedBySeverity[severity].Find(c => c.FullName.Equals(annotatedClasses[0].CodeSnippetId));
                    if (classForExport == null) continue;
                    var classFolderPath = _resultFolder + severity + "/" + i + "/";
                    Directory.CreateDirectory(classFolderPath);
                    for (var j = 0; j < classForExport.Members.Count; j++)
                    {
                        File.WriteAllText(classFolderPath + j + 1 + ".txt", classForExport.Members[j].SourceCode);
                    }
                }
            }
        }
    }
}