using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer.ViewModel;
using OfficeOpenXml;

namespace DataSetExplorer.DataSetSerializer
{
    public class DataSetWithAnnotationsExporter
    {
        private readonly string _templatePath = "../../../DataSetSerializer/Template/Dataset_Annotation_Template.xlsx";
        private readonly string _exportPath;
        //TODO: ColumnHeuristicsModel should be repurposed as some kind of HeuristicCatalog and put into the domain layer
        private readonly ColumnHeuristicsModel _requiredSmells;
        public DataSetWithAnnotationsExporter(string exportPath, ColumnHeuristicsModel smells)
        {
            _exportPath = exportPath;
            _requiredSmells = smells;
        }
        public void Export(DataSet project, string fileName)
        {
            var populatedExcel = PopulateTemplate(project);
            Serialize(fileName, populatedExcel);
        }

        private ExcelPackage PopulateTemplate(DataSet project)
        {
            var template = LoadTemplate(project.Url);
            foreach (var smell in _requiredSmells.GetSmells())
            {
                var sheet = template.Workbook.Worksheets.First(s => s.Name == smell.Value);
                var instances = smell.RelevantSnippetType().SelectMany(project.GetInstancesOfType).ToList();
                PopulateInstances(sheet, instances);
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
                var sheet = template.Workbook.Worksheets.Add(smell.Value, defaultSheet);
                sheet.Cells[2, 2].Value = smell.Value;
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
                sheet.Cells[2, 4, 4, 5].Copy(sheet.Cells[2, 6 + 2 * i, 4, 7 + 2 * i]);
                sheet.Cells[2, 4 + 2 * i].Value = heuristics[i];
            }
            sheet.Cells[2, 4 + 2*heuristics.Count].Value = "Custom heuristics.";
        }

        private void PopulateInstances(ExcelWorksheet sheet, List<DataSetInstance> instances)
        {
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 4 + i;
                sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                sheet.Cells[row, 2].Value = instances[i].Link;

                if (i == instances.Count - 1) break;
                var nextRow = row + 1;
                sheet.Cells[row + ":" + row].Copy(sheet.Cells[nextRow + ":" + nextRow]);
            }
        }

        private void Serialize(string fileName, ExcelPackage populatedExcel)
        {
            var filePath = _exportPath + fileName + ".xlsx";
            populatedExcel.SaveAs(new FileInfo(filePath));
        }
    }
}
