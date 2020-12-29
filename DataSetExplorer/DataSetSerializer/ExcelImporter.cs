using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataSetExplorer.DataSetBuilder.Model;
using OfficeOpenXml;

namespace DataSetExplorer.DataSetSerializer
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
        /// The excel documents must be formatted following these guidelines https://github.com/Clean-CaDET/platform-documentation/wiki/Dataset-Explorer#excel-import
        /// </summary>
        /// <param name="dataSetName">Name of the returned dataset and folder that contains the documents.</param>
        /// <returns>A dataset constructed from multiple excel documents.</returns>
        public DataSet Import(string dataSetName)
        {
            var dataSet = new DataSet(dataSetName);

            var sheets = GetWorksheets(GetExcelDocuments());
            foreach (var excelWorksheet in sheets)
            {
                dataSet.AddInstances(ExtractInstances(excelWorksheet));
            }

            return dataSet;
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

        private static List<DataSetInstance> ExtractInstances(ExcelWorksheet sheet)
        {
            var instances = new List<DataSetInstance>();
            for (var row = StartingInstanceRow; row < sheet.Dimension.End.Row; row++)
            {
                if (string.IsNullOrEmpty(sheet.Cells["A" + row].Text)) throw new InvalidOperationException("Rows contain empty value. Error at row " + row);
                var instance = GetBasicInstance(sheet, row);
                instance.AddAnnotation(GetAnnotation(sheet, row));
                instances.Add(instance);
            }

            return instances;
        }

        private static DataSetInstance GetBasicInstance(ExcelWorksheet sheet, int row)
        {
            var codeSnippetId = sheet.Cells["A" + row].Text;
            var link = sheet.Cells["B" + row].Text;
            var projectLink = sheet.Cells["A2"].Text;
            var snippetType = GetInstanceType(codeSnippetId);
            return new DataSetInstance(codeSnippetId, link, projectLink, snippetType);
        }

        private static SnippetType GetInstanceType(string codeSnippetId)
        {
            if (codeSnippetId.Contains("(")) return SnippetType.Function;
            return SnippetType.Class;
        }

        private static DataSetAnnotation GetAnnotation(ExcelWorksheet sheet, int row)
        {
            var smellSeverity = int.Parse(sheet.Cells["C" + row].Text);
            var annotatorId = int.Parse(sheet.Cells["C2"].Text);
            var codeSmell = sheet.Cells["B2"].Text;
            var heuristics = GetHeuristics(sheet, row);
            return new DataSetAnnotation(codeSmell, smellSeverity, annotatorId, heuristics);
        }

        private static List<SmellHeuristic> GetHeuristics(ExcelWorksheet sheet, int row)
        {
            var heuristics = new List<SmellHeuristic>();
            for (var col = StartingHeuristicColumn; col < sheet.Dimension.End.Column; col += 2)
            {
                if (sheet.Cells[row, col].Text != "Yes" && sheet.Cells[row, col].Text != "No") throw new InvalidOperationException("Columns are not properly formatted. Error at column " + col + " and row " + row);
                var isNotApplicable = sheet.Cells[row, col].Text == "No";
                if (isNotApplicable) continue;
                heuristics.Add(new SmellHeuristic(sheet.Cells[2, col].Text, sheet.Cells[row, col + 1].Text));
            }
            return heuristics;
        }
    }
}
