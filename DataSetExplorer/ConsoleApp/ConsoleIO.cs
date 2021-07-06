using DataSetExplorer.DataSetBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace DataSetExplorer.ConsoleApp
{
    internal class ConsoleIO
    {
        internal static void ChosenOptionErrorMessage(string chosenOption)
        {
            Console.WriteLine("Option " + chosenOption + " not found. Choose again.\n");
        }

        internal static string GetAnswerOnQuestion(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }

        internal static ListDictionary GetAnnotatedProjects()
        {
            ListDictionary projects = new ListDictionary();
            string localRepoPath;
            string annotationsFolderPath;
            string finishOption;

            Console.WriteLine("Projects:");
            do
            {
                localRepoPath = GetAnswerOnQuestion("Enter local repository folder path: ");
                annotationsFolderPath = GetAnswerOnQuestion("Enter annotations folder path: ");
                projects.Add(localRepoPath, annotationsFolderPath);
                finishOption = GetAnswerOnQuestion("Finished? (y/n): ");
            } while (finishOption.Equals("n"));

            return projects;
        }

        internal static List<Annotator> GetAnnotators()
        {
            List<Annotator> annotators = new List<Annotator>();
            string annotatorsFilePath = GetAnswerOnQuestion("Enter annotators file path: ");

            string[] lines = File.ReadAllLines(annotatorsFilePath);
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                annotators.Add(new Annotator(int.Parse(columns[0]), int.Parse(columns[1]), int.Parse(columns[2])));
            }
            return annotators;
        }
    }
}
