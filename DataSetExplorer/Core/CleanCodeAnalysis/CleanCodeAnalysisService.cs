using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.Core.DataSets;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;
using OfficeOpenXml;
using System.Collections.Generic;
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
                PopulateCleanNamesTemplate(instances[projectName]);
                Serialize(projectName + "_CleanNames");
            }
        }

        private void PopulateCleanNamesTemplate(List<Instance> instances)
        {
            var identifierCount = 0;
            instances.RemoveAll(i => i.Type.Equals(SnippetType.Function));
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
                var row = 3 + i;
                _sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                _sheet.Cells[row, 2].Value = instances[i].Link;

                _sheet.Cells[2, 3].Value = CaDETMetric.MLOC;
                _sheet.Cells[row, 3].Value = instances[i].MetricFeatures[CaDETMetric.MLOC];

                _sheet.Cells[2, 4].Value = CaDETMetric.CYCLO_SWITCH;
                _sheet.Cells[row, 4].Value = instances[i].MetricFeatures[CaDETMetric.CYCLO_SWITCH];

                _sheet.Cells[2, 5].Value = CaDETMetric.MMNB;
                _sheet.Cells[row, 5].Value = instances[i].MetricFeatures[CaDETMetric.MMNB];
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
                var row = 3 + i;
                _sheet.Cells[row, 1].Value = instances[i].CodeSnippetId;
                _sheet.Cells[row, 2].Value = instances[i].Link;

                _sheet.Cells[2, 3].Value = CaDETMetric.CLOC;
                _sheet.Cells[row, 3].Value = instances[i].MetricFeatures[CaDETMetric.CLOC];

                _sheet.Cells[2, 4].Value = CaDETMetric.WMC;
                _sheet.Cells[row, 4].Value = instances[i].MetricFeatures[CaDETMetric.WMC];

                _sheet.Cells[2, 5].Value = CaDETMetric.NAD;
                _sheet.Cells[row, 5].Value = instances[i].MetricFeatures[CaDETMetric.NAD];

                _sheet.Cells[2, 6].Value = CaDETMetric.NMD;
                _sheet.Cells[row, 6].Value = instances[i].MetricFeatures[CaDETMetric.NMD];

                _sheet.Cells[2, 7].Value = CaDETMetric.CBO;
                _sheet.Cells[row, 7].Value = instances[i].MetricFeatures[CaDETMetric.CBO];

                _sheet.Cells[2, 8].Value = CaDETMetric.DIT;
                _sheet.Cells[row, 8].Value = instances[i].MetricFeatures[CaDETMetric.DIT];
            }
        }

        private void Serialize(string fileName)
        {
            var filePath = _exportPath + fileName + ".xlsx";
            _excelFile.SaveAs(new FileInfo(filePath));
        }
    }
}
