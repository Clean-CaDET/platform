using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.Core.CleanCodeAnalysis.Model;
using DataSetExplorer.Core.DataSets;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DataSetExplorer.Core.CleanCodeAnalysis
{
    public class CleanCodeAnalysisService : ICleanCodeAnalysisService
    {
        private readonly string _cleanNamesAnalysisTemplatePath = "./Core/DataSetSerializer/Template/Clean_Names_Analysis_Template.xlsx";
        private readonly string _cleanFunctionsAnalysisTemplatePath = "./Core/DataSetSerializer/Template/Clean_Functions_Analysis_Template.xlsx";
        private readonly string _cleanClassesAnalysisTemplatePath = "./Core/DataSetSerializer/Template/Clean_Classes_Analysis_Template.xlsx";
        private string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;
        private readonly IInstanceService _instanceService;

        public CleanCodeAnalysisService(IInstanceService instanceService)
        {
            _instanceService = instanceService;
        }

        public Result<string> ExportDatasetAnalysis(int datasetId, CleanCodeAnalysisDTO analysisOptions)
        {
            _exportPath = analysisOptions.ExportPath;
            foreach (var option in analysisOptions.CleanCodeOptions)
            {
                if (option.Equals("Clean names"))
                    ExportCleanNamesAnalysis(_instanceService.GetInstancesWithIdentifiersByDatasetId(datasetId).Value);
                if (option.Equals("Clean functions"))
                    ExportCleanFunctionsAnalysis(_instanceService.GetInstancesByDatasetId(datasetId).Value);
                if (option.Equals("Clean classes"))
                    ExportCleanClassesAnalysis(_instanceService.GetInstancesByDatasetId(datasetId).Value);
            }
            return Result.Ok(analysisOptions.ExportPath);
        }

        public Result<string> ExportProjectAnalysis(int projectId, CleanCodeAnalysisDTO analysisOptions)
        {
            _exportPath = analysisOptions.ExportPath;
            foreach (var option in analysisOptions.CleanCodeOptions)
            {
                if (option.Equals("Clean names"))
                    ExportCleanNamesAnalysis(_instanceService.GetInstancesWithIdentifiersByProjectId(projectId).Value);
                if (option.Equals("Clean functions"))
                    ExportCleanFunctionsAnalysis(_instanceService.GetInstancesByProjectId(projectId).Value);
                if (option.Equals("Clean classes"))
                    ExportCleanClassesAnalysis(_instanceService.GetInstancesByProjectId(projectId).Value);
            }
            return Result.Ok(analysisOptions.ExportPath);
        }

        private void ExportCleanNamesAnalysis(Dictionary<string, List<Instance>> instances)
        {
            if (instances == default) return;

            foreach (var projectName in instances.Keys)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                _excelFile = new ExcelPackage(new FileInfo(_cleanNamesAnalysisTemplatePath));
                _sheet = _excelFile.Workbook.Worksheets[0];
                FilterInstances(instances[projectName]);
                PopulateCleanNamesTemplate(instances[projectName]);
                Serialize(projectName + "_CleanNames");
            }
        }

        private void FilterInstances(List<Instance> instances)
        {
            RemoveFunctions(instances);
            
        }

        private void RemoveFunctions(List<Instance> instances)
        {
            instances.RemoveAll(i => i.Type.Equals(SnippetType.Function));
        }

        private void PopulateCleanNamesTemplate(List<Instance> instances)
        {
            var identifierCount = 0;
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 3 + i + identifierCount;
                _sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                _sheet.Cells[row, 2].Value = instances[i].Link;

                instances[i].Identifiers.Sort((x, y) => x.Type.CompareTo(y.Type));
                for (var j = 0; j < instances[i].Identifiers.Count; j++)
                {
                    _sheet.Cells[row + j, 3].Value = instances[i].Identifiers[j].Name;
                    _sheet.Cells[row + j, 4].Value = instances[i].Identifiers[j].Type.ToString();
                }
                identifierCount += instances[i].Identifiers.Count;
            }
        }

        private void ExportCleanFunctionsAnalysis(Dictionary<string, List<Instance>> instances)
        {
            if (instances == default) return;

            foreach (var projectName in instances.Keys)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                _excelFile = new ExcelPackage(new FileInfo(_cleanFunctionsAnalysisTemplatePath));
                _sheet = _excelFile.Workbook.Worksheets[0];
                PopulateCleanFunctionsTemplate(instances[projectName]);
                Serialize(projectName + "_CleanFunctions");
            }
        }

        private void PopulateCleanFunctionsTemplate(List<Instance> instances)
        {
            instances.RemoveAll(i => i.Type.Equals(SnippetType.Class));
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 5 + i;
                PopulateInstanceInfo(row, instances[i]);

                PopulateMetrics(row, 3, CaDETMetric.MLOC, 15, 25, instances[i]);
                SetConditionalFormatting(new ExcelAddress(5, 3, instances.Count + 4, 3), _sheet.Cells[3, 3].Value, Color.Red);
                SetConditionalFormatting(new ExcelAddress(5, 3, instances.Count + 4, 3), _sheet.Cells[2, 3].Value, Color.Yellow);

                PopulateMetrics(row, 4, CaDETMetric.CYCLO_SWITCH, 6, 12, instances[i]);
                SetConditionalFormatting(new ExcelAddress(5, 4, instances.Count + 4, 4), _sheet.Cells[3, 4].Value, Color.Red);
                SetConditionalFormatting(new ExcelAddress(5, 4, instances.Count + 4, 4), _sheet.Cells[2, 4].Value, Color.Yellow);

                PopulateMetrics(row, 5, CaDETMetric.MMNB, 4, 6, instances[i]);
                SetConditionalFormatting(new ExcelAddress(5, 5, instances.Count + 4, 5), _sheet.Cells[3, 5].Value, Color.Red);
                SetConditionalFormatting(new ExcelAddress(5, 5, instances.Count + 4, 5), _sheet.Cells[2, 5].Value, Color.Yellow);
            }
        }

        private void ExportCleanClassesAnalysis(Dictionary<string, List<Instance>> instances)
        {
            if (instances == default) return;

            foreach (var projectName in instances.Keys)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                _excelFile = new ExcelPackage(new FileInfo(_cleanClassesAnalysisTemplatePath));
                _sheet = _excelFile.Workbook.Worksheets[0];
                PopulateCleanClassesTemplate(instances[projectName]);
                Serialize(projectName + "_CleanClasses");
            }
        }

        private void PopulateCleanClassesTemplate(List<Instance> instances)
        {
            instances.RemoveAll(i => i.Type.Equals(SnippetType.Function));
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 5 + i;
                PopulateInstanceInfo(row, instances[i]);

                PopulateMetrics(row, 3, CaDETMetric.CLOC, 80, 150, instances[i]);
                SetConditionalFormatting(new ExcelAddress(5, 3, instances.Count + 4, 3), _sheet.Cells[3, 3].Value, Color.Red);
                SetConditionalFormatting(new ExcelAddress(5, 3, instances.Count + 4, 3), _sheet.Cells[2, 3].Value, Color.Yellow);

                PopulateMetrics(row, 4, CaDETMetric.WMC, 15, 25, instances[i]);
                SetConditionalFormatting(new ExcelAddress(5, 4, instances.Count + 4, 4), _sheet.Cells[3, 4].Value, Color.Red);
                SetConditionalFormatting(new ExcelAddress(5, 4, instances.Count + 4, 4), _sheet.Cells[2, 4].Value, Color.Yellow);

                PopulateMetrics(row, 5, CaDETMetric.NAD, 10, 15, instances[i]);
                SetConditionalFormatting(new ExcelAddress(5, 5, instances.Count + 4, 5), _sheet.Cells[3, 5].Value, Color.Red);
                SetConditionalFormatting(new ExcelAddress(5, 5, instances.Count + 4, 5), _sheet.Cells[2, 5].Value, Color.Yellow);

                PopulateMetrics(row, 6, CaDETMetric.NMD, 12, 16, instances[i]);
                SetConditionalFormatting(new ExcelAddress(5, 6, instances.Count + 4, 6), _sheet.Cells[3, 6].Value, Color.Red);
                SetConditionalFormatting(new ExcelAddress(5, 6, instances.Count + 4, 6), _sheet.Cells[2, 6].Value, Color.Yellow);

                PopulateMetrics(row, 7, CaDETMetric.CBO, 8, 12, instances[i]);
                SetConditionalFormatting(new ExcelAddress(5, 7, instances.Count + 4, 7), _sheet.Cells[3, 7].Value, Color.Red);
                SetConditionalFormatting(new ExcelAddress(5, 7, instances.Count + 4, 7), _sheet.Cells[2, 7].Value, Color.Yellow);

                PopulateMetrics(row, 8, CaDETMetric.DIT, 3, 5, instances[i]);
                SetConditionalFormatting(new ExcelAddress(5, 8, instances.Count + 4, 8), _sheet.Cells[3, 8].Value, Color.Red);
                SetConditionalFormatting(new ExcelAddress(5, 8, instances.Count + 4, 8), _sheet.Cells[2, 8].Value, Color.Yellow);
            }
        }

        private void PopulateInstanceInfo(int row, Instance instance)
        {
            _sheet.Cells[row, 1].Value = instance.CodeSnippetId;
            _sheet.Cells[row, 2].Value = instance.Link;
        }

        private void PopulateMetrics(int row, int column, CaDETMetric metric, int suspiciousValue, int criticalValue, Instance instance)
        {
            _sheet.Cells[2, column].Value = suspiciousValue;
            _sheet.Cells[3, column].Value = criticalValue;
            _sheet.Cells[4, column].Value = metric;
            _sheet.Cells[row, column].Value = instance.MetricFeatures[metric];
        }

        private void SetConditionalFormatting(ExcelAddress excelAddress, object formula, Color color)
        {
            var cf = _sheet.ConditionalFormatting.AddGreaterThan(excelAddress);
            cf.Formula = formula.ToString();
            cf.Style.Fill.BackgroundColor.Color = color;
        }

        private void Serialize(string fileName)
        {
            var filePath = _exportPath + fileName + ".xlsx";
            _excelFile.SaveAs(new FileInfo(filePath));
        }
    }
}
