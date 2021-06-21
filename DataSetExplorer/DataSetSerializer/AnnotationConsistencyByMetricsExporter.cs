using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSetBuilder.Model;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExplorer.DataSetSerializer
{
    class AnnotationConsistencyByMetricsExporter
    {
        private readonly string _singleAnnotatorTemplatePath = "../../../DataSetSerializer/Template/Single_Annotator_Consistency_Template.xlsx";
        private readonly string _multipleAnnotatorsTemplatePath = "../../../DataSetSerializer/Template/Consistency_Between_Annotators_Template.xlsx";
        private readonly string _exportPath;
        private ExcelPackage _excelFile;
        private ExcelWorksheet _sheet;

        public AnnotationConsistencyByMetricsExporter(string exportPath)
        {
            _exportPath = exportPath;
        }

        public void ExportAnnotationsFromAnnotator(int annotatorId, List<DataSetInstance> instances,
            string fileName)
        {
            InitializeExcelSheet(_singleAnnotatorTemplatePath);
            PopulateTemplateForAnnotator(annotatorId, instances);
            Serialize(fileName);
        }

        public void ExportAnnotatorsForSeverity(int severity, List<DataSetInstance> instances,
            string fileName)
        {
            InitializeExcelSheet(_multipleAnnotatorsTemplatePath);
            PopulateTemplateForSeverity(severity, instances);
            Serialize(fileName);
        }

        private void InitializeExcelSheet(string templatePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _excelFile = new ExcelPackage(new FileInfo(templatePath));
            _sheet = _excelFile.Workbook.Worksheets[0];
        }

        private void PopulateTemplateForAnnotator(int annotatorId, List<DataSetInstance> instances)
        {
            for (var i = 0; i < instances.Count; i++)
            {
                var row = 2 + i;
                _sheet.Cells[row, 1].Value = instances[i].Annotations.First(a => a.Annotator.Id == annotatorId).Severity;
                PopulateMetrics(instances[i].MetricFeatures, row, 2);
            }
        }

        private void PopulateTemplateForSeverity(int severity, List<DataSetInstance> instances)
        {
            var j = 0;
            foreach (var instance in instances)
            {
                foreach (var annotation in instance.Annotations)
                {
                    if (annotation.Severity == severity)
                    {
                        _sheet.Cells[2 + j, 1].Value = annotation.Annotator.Id;
                        PopulateMetrics(instance.MetricFeatures, 2 + j, 2);
                        j++;
                    }
                }
            }
        }

        private void PopulateMetrics(Dictionary<CaDETMetric, double> metrics, int row, int startColumn)
        {
            var i = 0;
            foreach (var key in metrics.Keys)
            {
                _sheet.Cells[1, startColumn + i].Value = key;
                _sheet.Cells[row, startColumn + i].Value = metrics[key];
                i++;
            }
        }

        private void Serialize(string fileName)
        {
            var filePath = _exportPath + fileName + ".xlsx";
            _excelFile.SaveAs(new FileInfo(filePath));
        }
    }
}
