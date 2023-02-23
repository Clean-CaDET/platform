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
        private readonly string _cleanClassesAnalysisTemplatePath = "./Core/DataSetSerializer/Template/Clean_Classes_Template.xlsx";
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
                    ExportCleanNamesAnalysis(_instanceService.GetAllByDatasetId(datasetId).Value);
                if (option.Equals("Clean functions"))
                    ExportCleanFunctionsAnalysis();
                if (option.Equals("Clean classes"))
                    ExportCleanClassesAnalysis();
            }
            return Result.Ok(analysisOptions.ExportPath);
        }

        public Result<string> ExportProjectAnalysis(int projectId, CleanCodeAnalysisDTO analysisOptions)
        {
            _exportPath = analysisOptions.ExportPath;
            foreach (var option in analysisOptions.CleanCodeOptions)
            {
                if (option.Equals("Clean names"))
                    ExportCleanNamesAnalysis(_instanceService.GetAllByProjectId(projectId).Value);
                if (option.Equals("Clean functions"))
                    ExportCleanFunctionsAnalysis();
                if (option.Equals("Clean classes"))
                    ExportCleanClassesAnalysis();
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

        private void ExportCleanFunctionsAnalysis()
        {
            // TODO
        }

        private void ExportCleanClassesAnalysis()
        {
            // TODO
        }

        private void Serialize(string fileName)
        {
            var filePath = _exportPath + fileName + ".xlsx";
            _excelFile.SaveAs(new FileInfo(filePath));
        }
    }
}
