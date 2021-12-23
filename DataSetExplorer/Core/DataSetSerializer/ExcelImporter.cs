using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using OfficeOpenXml;

namespace DataSetExplorer.Core.DataSetSerializer
{
    public class ExcelImporter
    {
        private const int StartingInstanceRow = 4;
        private const int StartingHeuristicColumn = 4;
        private readonly string _sourceFolder;

        public ExcelImporter(string sourceFolder)
        {
            _sourceFolder = sourceFolder;
        }

        /// <summary>
        /// This logic is highly dependent on the appropriate excel file structure.
        /// It examines excel documents in the sourceFolder directory and its subdirectories.
        /// The excel documents must be formatted following these guidelines https://github.com/Clean-CaDET/platform/wiki/Dataset-Explorer#building-your-dataset
        /// </summary>
        /// <param name="projectName">Name of the returned dataset project.</param>
        /// <returns>A dataset project constructed from one or more excel documents.</returns>
        public DataSetProject Import(string projectName)
        {
            var project = new DataSetProject(projectName);

            var sheets = GetWorksheets(GetExcelDocuments());
            foreach (var excelWorksheet in sheets)
            {
                project.AddCandidateInstance(new SmellCandidateInstances(new CodeSmell(excelWorksheet.Name), ExtractInstances(excelWorksheet)));
            }
            
            return project;
        }

        private IEnumerable<ExcelPackage> GetExcelDocuments()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var dataSetFiles = Directory.GetFiles(_sourceFolder, "*.xlsx", SearchOption.AllDirectories);
            return dataSetFiles.Select(path => new ExcelPackage(new FileInfo(path)));
        }

        private static List<ExcelWorksheet> GetWorksheets(IEnumerable<ExcelPackage> documents)
        {
            var sheets = new List<ExcelWorksheet>();
            foreach (var document in documents)
            {
                sheets.AddRange(document.Workbook.Worksheets);
            }

            return sheets;
        }

        private static List<Instance> ExtractInstances(ExcelWorksheet sheet)
        {
            var instances = new List<Instance>();
            for (var row = StartingInstanceRow; row <= sheet.Dimension.End.Row; row++)
            {
                if (IsEndOfSheet(sheet, row)) break;
                var instance = GetBasicInstance(sheet, row);
                instance.AddAnnotation(GetAnnotation(sheet, row));
                instances.Add(instance);
            }

            return instances;
        }

        private static bool IsEndOfSheet(ExcelWorksheet sheet, int row)
        {
            return string.IsNullOrEmpty(sheet.Cells["A" + row].Text);
        }

        private static Instance GetBasicInstance(ExcelWorksheet sheet, int row)
        {
            var codeSnippetId = sheet.Cells["A" + row].Text;
            var link = sheet.Cells["B" + row].Text;
            var projectLink = sheet.Cells["A2"].Text;
            var snippetType = GetInstanceType(codeSnippetId);
            return new Instance(codeSnippetId, link, projectLink, snippetType, null);
        }

        private static SnippetType GetInstanceType(string codeSnippetId)
        {
            if (codeSnippetId.Contains("(")) return SnippetType.Function;
            return SnippetType.Class;
        }

        internal List<Instance> ImportAnnotatedInstancesFromDataSet(string path)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var excel = new ExcelPackage(new FileInfo(path));
            var sheets = excel.Workbook.Worksheets;
            var instances = new List<Instance>();
            foreach (var sheet in sheets)
            {
                for (var row = 3; row <= sheet.Dimension.End.Row; row++)
                {
                    if (IsEndOfSheet(sheet, row)) break;
                    var codeSnippetId = sheet.Cells["A" + row].Text;
                    var projectLink = sheet.Cells["D" + row].Text;
                    var finalAnnotation = sheet.Cells["AD" + row].Text;
                    Instance instance = new Instance(codeSnippetId, projectLink);
                    // Dummy values for DataSetAnnotation constructor to pass validations (the final annotation is the only important parameter in this case).
                    Annotation annotation = new Annotation(new CodeSmell("Large_Class"), int.Parse(finalAnnotation), new Annotator(1), new List<SmellHeuristic>() { new SmellHeuristic("", true, "") });
                    instance.AddAnnotation(annotation);
                    instances.Add(instance);
                }
            }
            return instances;
        }

        private static Annotation GetAnnotation(ExcelWorksheet sheet, int row)
        {
            try
            {
                var smellSeverity = int.Parse(sheet.Cells["C" + row].Text);
                var annotatorId = int.Parse(sheet.Cells["C2"].Text);
                var codeSmell = sheet.Cells["B2"].Text;
                var heuristics = GetHeuristics(sheet, row);
                return new Annotation(codeSmell, smellSeverity, new Annotator(annotatorId), heuristics);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(GetErrorMessage(e.Message, sheet, row, 1));
            }
            catch (FormatException)
            {
                throw new InvalidOperationException(GetErrorMessage("Severity or annotator ID empty", sheet, row, 2));
            }
        }

        private static List<SmellHeuristic> GetHeuristics(ExcelWorksheet sheet, int row)
        {
            var heuristics = new List<SmellHeuristic>();
            for (var col = StartingHeuristicColumn; col < sheet.Dimension.End.Column; col += 2)
            {
                if (sheet.Cells[row, col].Text != "Yes" && sheet.Cells[row, col].Text != "No") throw new InvalidOperationException(GetErrorMessage("Yes or No allowed.", sheet, row, col));
                var isApplicable = sheet.Cells[row, col].Text == "Yes";
                heuristics.Add(new SmellHeuristic(sheet.Cells[2, col].Text, isApplicable, sheet.Cells[row, col + 1].Text));
            }
            return heuristics;
        }

        private static string GetErrorMessage(string postfixMessage, ExcelWorksheet sheet, int row, int col)
        {
            return "Error at column " + col + " and row " + row +
                   " for smell " + sheet.Cells["B2"].Text + " and annotator "
                   + sheet.Cells["C2"].Text + ". " + postfixMessage;
        }
    }
}
