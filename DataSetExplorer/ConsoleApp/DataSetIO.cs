using DataSetExplorer.DataSets.Model;
using System.Collections.Generic;
using System.IO;

namespace DataSetExplorer.ConsoleApp
{
    internal class DataSetIO
    {
        internal static IDictionary<string, string> GetProjects(string info)
        {
            var projects = new Dictionary<string, string>();
            string projectsFilePath = ConsoleIO.GetAnswerOnQuestion("Enter projects file (csv with " + info + ") path: ");

            string[] lines = File.ReadAllLines(projectsFilePath);
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                projects[columns[0]] = columns[1];
            }
            return projects;
        }

        internal static List<Annotator> GetAnnotators()
        {
            List<Annotator> annotators = new List<Annotator>();
            string annotatorsFilePath = ConsoleIO.GetAnswerOnQuestion("Enter annotators file (csv with annotator id, years of experience and rank) path: ");

            string[] lines = File.ReadAllLines(annotatorsFilePath);
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                annotators.Add(new Annotator(int.Parse(columns[0]), int.Parse(columns[1]), int.Parse(columns[2])));
            }
            return annotators;
        }

        internal static List<CodeSmell> GetCodeSmells(string info)
        {
            var codeSmells = new List<CodeSmell>();
            string codeSmellsFilePath = ConsoleIO.GetAnswerOnQuestion("Enter code smells file (csv with " + info + ") path: ");

            string[] lines = File.ReadAllLines(codeSmellsFilePath);
            foreach (string line in lines)
            {
                codeSmells.Add(new CodeSmell(line));
            }
            return codeSmells;
        }
    }
}
