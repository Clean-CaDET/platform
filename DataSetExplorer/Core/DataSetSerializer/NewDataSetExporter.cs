using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSetSerializer.ViewModel;
using OfficeOpenXml;

namespace DataSetExplorer.Core.DataSetSerializer
{
    public class NewDataSetExporter
    {
        private readonly string _templatePath = "./DataSetSerializer/Template/New_Dataset_Template.xlsx";
        private readonly string _exportPath;
        //TODO: ColumnHeuristicsModel should be repurposed as some kind of HeuristicCatalog and put into the domain layer
        private readonly ColumnHeuristicsModel _requiredSmells;
        private readonly bool _includeMetrics;
        public NewDataSetExporter(string exportPath, NewSpreadSheetColumnModel columnModel)
        {
            _exportPath = exportPath;
            _requiredSmells = columnModel.SmellsAndHeuristics;
            _includeMetrics = columnModel.IncludeMetrics;
        }
        public void Export(DataSet dataSet, string fileName)
        {
            foreach (var project in dataSet.Projects)
            {
                var populatedExcel = PopulateTemplate(dataSet, project);
                Serialize(fileName, project.Name, populatedExcel);
            }
        }

        private ExcelPackage PopulateTemplate(DataSet dataSet, DataSetProject project)
        {
            var template = LoadTemplate(project.Url);
            foreach (var smell in _requiredSmells.GetSmells())
            {
                var sheet = template.Workbook.Worksheets.First(s => s.Name == smell.Name);
                var dataSetProject = dataSet.Projects.First(p => p.Name.Equals(project.Name));
                var instances = dataSetProject.CandidateInstances.First(c => c.CodeSmell.Name.Equals(smell.Name)).Instances;
                PopulateInstances(sheet, instances, smell);
            }

            return template;
        }

        private ExcelPackage LoadTemplate(string projectUrl)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var template = new ExcelPackage(new FileInfo(_templatePath));
            
            var defaultSheet = template.Workbook.Worksheets[0];
            defaultSheet.Cells[2, 1].Value = projectUrl;
            
            foreach (var smell in _requiredSmells.GetSmells())
            {
                var sheet = template.Workbook.Worksheets.Add(smell.Name, defaultSheet);
                sheet.Cells[2, 2].Value = smell.Name;
                PopulateHeuristics(sheet, smell);
            }

            template.Workbook.Worksheets.Delete(0);
            return template;
        }

        private void PopulateHeuristics(ExcelWorksheet sheet, CodeSmell smell)
        {
            var heuristics = _requiredSmells.GetHeuristics(smell);
            for (var i = 0; i < heuristics.Count; i++)
            {
                sheet.Cells[2, 4, 4, 5].Copy(sheet.Cells[2, 6 + (2 * i), 4, 7 + (2 * i)]);
                sheet.Cells[2, 4 + (2 * i)].Value = heuristics[i];
            }
            sheet.Cells[2, 4 + (2*heuristics.Count)].Value = "Custom heuristics.";
        }

        private void PopulateInstances(ExcelWorksheet sheet, List<Instance> instances, CodeSmell smell)
        {
            int numOfHeuristics = _requiredSmells.GetHeuristics(smell).Count() + 1; // +1 for custom heuristic
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 4 + i;
                sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                sheet.Cells[row, 2].Value = instances[i].Link;
                if (_includeMetrics) PopulateMetrics(sheet, instances[i], row, 4 + (numOfHeuristics*2));

                if (i == instances.Count - 1) break;
                var nextRow = row + 1;
                sheet.Cells[row + ":" + row].Copy(sheet.Cells[nextRow + ":" + nextRow]);
            }
        }

        private void PopulateMetrics(ExcelWorksheet sheet, Instance dataSetInstance, int row, int column)
        {
            sheet.Cells[1, column].Value = "Metrics";
            foreach (var metric in dataSetInstance.MetricFeatures)
            {
                sheet.Cells[3, column].Value = metric.Key;
                sheet.Cells[row, column].Value = metric.Value;
                column++;
            }
        }

        private void Serialize(string fileName, string projectName, ExcelPackage populatedExcel)
        {
            var filePath = _exportPath + fileName + "_" + projectName + ".xlsx";
            populatedExcel.SaveAs(new FileInfo(filePath));
        }
    }
}
