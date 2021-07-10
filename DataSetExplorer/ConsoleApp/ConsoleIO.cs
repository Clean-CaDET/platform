using System;

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
    }
}
